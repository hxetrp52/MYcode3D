using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private float startSpeed;
    private Animator playerAnimator;
    public Rigidbody playerRb;
    public PhotonView pv;

    public bool canMove = true;
    public float inputZ;
    public float inputX;
    private FlashControl flashControl;
    private ProximitySelector proximitySelector;


    private void Awake()
    {
        Application.targetFrameRate = 120; //프레임 설정
    }
    // Start is called before the first frame update
    void Start()
    {
        DialogueManager.Instance.ResetDatabase();
        flashControl = GetComponent<FlashControl>(); 
        pv = GetComponent<PhotonView>();
        startSpeed = speed;
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        proximitySelector = GetComponent<ProximitySelector>();
        DialogueManager.instance.conversationStarted += OnConversationStarted;
        DialogueManager.instance.conversationEnded += OnConversationEnded;
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            proximitySelector.enabled = true;
        }
        else
        {
            proximitySelector.enabled = false;
        }
        inputZ = Input.GetAxisRaw("Vertical"); // 앞 뒤 입력
        inputX = Input.GetAxisRaw("Horizontal"); // 좌우 입력
        var dir = Vector3.forward;
        var side = Vector3.right;


        if (canMove)
        {
            if (pv.IsMine)
            {
                #region movement
                playerRb.MovePosition(playerRb.position + transform.TransformDirection(side) * (inputX * (speed / 2) * Time.deltaTime));
                if (inputZ > 0)
                {
                    playerRb.MovePosition(playerRb.position + transform.TransformDirection(dir) * (inputZ * speed * Time.deltaTime));
                }
                else if (inputZ < 0)
                {
                    playerRb.MovePosition(playerRb.position + transform.TransformDirection(dir) * ((inputZ + 0.5f) * speed * Time.deltaTime));

                }
                #endregion

                #region move_animation
                if (inputZ != 0 || inputX != 0)
                {
                    playerAnimator.SetBool("isMove", true);
                }
                else
                {
                    playerAnimator.SetBool("isMove", false);
                }
                playerAnimator.SetFloat("Zdir", inputZ);
                playerAnimator.SetFloat("Xdir", inputX);
                #endregion

                #region run
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speed = startSpeed * 2;
                    playerAnimator.speed = 2;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    playerAnimator.speed = 1;
                    speed = startSpeed;
                }
                #endregion
                
            }
        }
        else
        {
            playerAnimator.SetBool("isMove", false);
        }
    }

    void OnConversationStarted(Transform actor) // 대화중일때 못움직이게 하기
    {
        canMove = false;
        flashControl.OnFlash(false);
        flashControl.canflash = false;
    }

    void OnConversationEnded(Transform actor)
    {
        canMove = true;
        flashControl.canflash = true;
        flashControl.flashBody.gameObject.SetActive(true);
        flashControl.flashSlider.gameObject.SetActive(true);
        speed = startSpeed;
    }
    [PunRPC]
    public void RPCOnFlash(bool ON)
    {
        flashControl.OnFlash(ON);
    }

    [PunRPC]
    private void RPCRotateFlashlight(float rotationY)
    {
        flashControl.flashLight.transform.localRotation = Quaternion.Euler(10, rotationY, 0);
    }

    void OnDisable()
    {
        // 이벤트 리스너 제거
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationStarted -= OnConversationStarted;
            DialogueManager.instance.conversationEnded -= OnConversationEnded;
        }
    }

    public void Test()
    {
        Debug.Log("게임시작");
    }
}
