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
        if (!playerMovement.pv.IsMine) return; 
        packCount.text = "배터리 팩 : " +flashPack.ToString()+" / 3";
        flashText.text = "배터리 : "+flashSlider.value.ToString("F0")+"%";
        if (canflash) 
        {
            UpdateFlashRotation();
        }
        if (Input.GetKeyDown(KeyCode.Q) &&  canflash)
        {
            playerMovement.pv.RPC("RPCOnFlash", RpcTarget.All, !isFlash);
        }
        if (isFlash)
        {
            if (flashSlider.value <= 0)
            {
                playerMovement.pv.RPC("RPCOnFlash", RpcTarget.All, false);
                canflash = false;
                flashText.color = Color.red;
            }
            else 
            { 
               flashSlider.value -= 4f * Time.deltaTime;
            }
        }
        if (flashPack > 0 && Input.GetKeyDown(KeyCode.R) && flashSlider.value == 0)
        {
           flashPack--;
           flashText.color = Color.white;
           flashSlider.value = 100;
           canflash = true;
        }
        else if(flashPack <= 0)
        {
            packCount.color = Color.red;
        }

    }

    void UpdateFlashRotation()
    {
        if (playerMovement.pv.IsMine)
        {
            float rotationY = CMcontoller.MouseY;
            playerMovement.pv.RPC("RPCRotateFlashlight", RpcTarget.All, rotationY);
        }
    }


    public void OnFlash(bool ON)
    {
        flashLight.SetActive(ON);
        isFlash = ON;
    }
}
