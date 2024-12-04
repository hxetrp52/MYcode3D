using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class FlashControl : MonoBehaviourPunCallbacks
{
    public GameObject flashLight;
    public GameObject flashBody;
    public Slider flashSlider;
    public TMP_Text packCount;
    public TMP_Text flashText;
    public AudioSource flashAudio;
    public bool canflash = false;
    private bool isFlash = false;
    private int flashPack = 3;
    private PlayerMovement playerMovement;
    [SerializeField] private CameraContoller CMcontoller;
    // Start is called before the first frame update
    void Start()
    {
        flashPack = 3;
        flashLight.SetActive(false);
        playerMovement = GetComponentInParent<PlayerMovement>();
        flashSlider.value = 100;
        flashSlider.gameObject.SetActive(false);
        flashBody.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMovement.pv.IsMine) return; //포톤 자기 것만 작동하기
        packCount.text = "배터리 팩 : " +flashPack.ToString()+" / 3";
        flashText.text = "배터리 : "+flashSlider.value.ToString("F0")+"%";
        if (canflash) 
        {
            UpdateFlashRotation(); // 마우스에 따라 불빛 회저 
        }
        if (Input.GetKeyDown(KeyCode.Q) &&  canflash) // 플레이어 불빛 동기화
        {
            playerMovement.pv.RPC("RPCOnFlash", RpcTarget.All, !isFlash);
            flashAudio.Play();
        }
        if (isFlash)
        {
            if (flashSlider.value <= 0) // 손전등 불 다 떨어졌을떄 비활성화
            {
                playerMovement.pv.RPC("RPCOnFlash", RpcTarget.All, false);
                canflash = false;
                flashText.color = Color.red;
            }
            else 
            { 
               flashSlider.value -= 4f * Time.deltaTime;   // 손전등이 켜져있을 떄 게이지 닳기  
            }
        }
        if (flashPack > 0 && Input.GetKeyDown(KeyCode.R) && flashSlider.value == 0) // 배터리 팩 교체하기
        {
           flashPack--;
           flashText.color = Color.white;
           flashSlider.value = 100;
           canflash = true;
        }
        else if(flashPack <= 0) // 배터리 팩 없을 때 색갈 바꾸기
        {
            packCount.color = Color.red;
        }

    }

    void UpdateFlashRotation() // 플레이어의 마우스 Y축 값에따라 불빛 방향 조절 동기화
    {
        if (playerMovement.pv.IsMine)
        {
            float rotationY = CMcontoller.MouseY;
            playerMovement.pv.RPC("RPCRotateFlashlight", RpcTarget.All, rotationY);
        }
    }


    public void OnFlash(bool ON) // 플레이어마다 불빛 동기화
    {
        flashLight.SetActive(ON);
        isFlash = ON;
    }
}
