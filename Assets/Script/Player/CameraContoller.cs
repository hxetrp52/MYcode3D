using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;

public class CameraContoller : MonoBehaviour
{
    public float mouseSensitivity = 400f; //마우스감도

    private float MouseY; //마우스 y축
    private float MouseX; //마우스 x축
    private bool canMove = true; // 카메라 이동 고정
    public GameObject Player; // 플레이어
    public GameObject FreshLight; // 손전등 불빛

    private void Start()
    {
        Player = transform.parent.gameObject; // 플레이어 참조
        DialogueManager.instance.conversationStarted += OnConversationStarted; // 다이어로그 에셋
        DialogueManager.instance.conversationEnded += OnConversationEnded;
    }
    private void Update()
    {
        if (canMove) // 이동 고정이 아닐때 회전 실행
        {
            Rotate();
        }
    }
    private void Rotate()
    {

        MouseX += Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime; // 마우스 위치받기

        MouseY -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime; // 마우스 위치받기
        
        Player.transform.rotation = Quaternion.Euler(0, MouseX, 0); // 마우스의 x축에 따라 플레이어 회전

        MouseY = Mathf.Clamp(MouseY, -20f, 30f); //Clamp를 통해 최소값 최대값을 넘지 않도록함 // 위 아래 보는거 최대 최소 설정

        FreshLight.transform.localRotation = Quaternion.Euler(0, MouseY, 0); // 후레쉬 불빛 화면이랑 같이 움직이기
        transform.rotation = Quaternion.Euler(MouseY, MouseX, 0f);// 각 축을 한꺼번에 계산 카메라 움직이기
    }

    void OnConversationStarted(Transform actor)
    {
        canMove = false;
    }

    void OnConversationEnded(Transform actor)
    {
        canMove = true;
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

}
