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
    public AudioSource playerAudio;

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
        playerAudio.enabled = false;
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
                playerRb.MovePosition(playerRb.position + transform.TransformDirection(side) * (inputX * (speed / 2) * Time.deltaTime)); //좌우 이동
                if (inputZ > 0) //앞으로 이동
                {
                    playerRb.MovePosition(playerRb.position + transform.TransformDirection(dir) * (inputZ * speed * Time.deltaTime));
                }
                else if (inputZ < 0) // 뒤로 이동시 감속
                {
                    playerRb.MovePosition(playerRb.position + transform.TransformDirection(dir) * ((inputZ + 0.5f) * speed * Time.deltaTime));

                }
                #endregion

                #region move_animation
                if (inputZ != 0 || inputX != 0) // 이동중인지 판단 후 애니메이션 적용
                {
                    playerAnimator.SetBool("isMove", true); 
                    playerAudio.enabled = true;
                }
                else
                {
                    playerAudio.enabled = false;
                    playerAnimator.SetBool("isMove", false);
                }
                playerAnimator.SetFloat("Zdir", inputZ);
                playerAnimator.SetFloat("Xdir", inputX);
                #endregion

                #region run
                if (Input.GetKey(KeyCode.LeftShift)) // 달리기
                {
                    speed = startSpeed * 2;
                    playerAnimator.speed = 2;
                    playerAudio.pitch = 1.4f;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift)) // 달리기 종료
                {
                    playerAnimator.speed = 1;
                    speed = startSpeed;
                    playerAudio.pitch = 1;
                }
                #endregion
                
            }
        }
        else
        {
            playerAnimator.SetBool("isMove", false);
            playerAudio.enabled = false;
        }
    }

    void OnConversationStarted(Transform actor) // 대화중일때 못움직이게 하기
    {
        canMove = false;
        flashControl.canflash = false;
    }

    void OnConversationEnded(Transform actor) // 지금 보면 ㅈㄴ 못짰다
    {
        canMove = true;
        flashControl.canflash = true;
        flashControl.flashBody.gameObject.SetActive(true);
        flashControl.flashSlider.gameObject.SetActive(true);
        speed = startSpeed;
    }
    [PunRPC] // 자식오브젝트에 포톤뷰 넣기 싫어서 부모에 몰아 넣으면 생기는 일
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
