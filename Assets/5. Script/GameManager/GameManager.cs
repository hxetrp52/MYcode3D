using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;

    void Start()
    {
        if (PhotonNetwork.IsMessageQueueRunning)
        {
            CreatePlayer();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnJoinedRoom()
    {
        CreatePlayer();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    void CreatePlayer()
    {
        if (!PhotonNetwork.InRoom) return;

        Transform spawnPoint = GameObject.Find("SpawnPoint").transform;
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
    }

        
}
