using Photon.Pun;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        OnActiveNPC(false);
    }

    [PunRPC]
    public void ActiveNPC()
    {
        OnActiveNPC(true);
    }

    private void OnActiveNPC(bool ON)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(ON);
        }
    }
}
