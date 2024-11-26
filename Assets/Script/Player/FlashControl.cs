using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlashControl : MonoBehaviour
{
    public GameObject flashLight;
    public Slider flashSlider;
    public TMP_Text packCount;
    public TMP_Text flashText;
    public bool canflash = false;
    private bool isFalsh = false;
    private int flashPack = 3;
    // Start is called before the first frame update
    void Start()
    {
        flashSlider.value = 100;
        flashPack = 3;
        flashLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        packCount.text = "배터리 팩 : " +flashPack.ToString()+" / 3";
        flashText.text = "배터리 : "+flashSlider.value.ToString("F0")+"%";
        if (Input.GetKeyDown(KeyCode.Q) &&  canflash)
        {
            if (!isFalsh)
            {
                OnFlash(true);
            }
            else
            {
                OnFlash(false);
            }
        }
        if (isFalsh)
        {
            if (flashSlider.value <= 0)
            {
                OnFlash(false);
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

    public void OnFlash(bool ON)
    {
        flashLight.SetActive(ON);
        isFalsh = ON;
    }
}
