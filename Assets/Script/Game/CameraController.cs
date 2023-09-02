using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CameraController : MonoBehaviourPunCallbacks
{
    private bool firstTime = true;
    public int w;
    public int h;
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;
    public GameObject UIText;
    Text text;
    //
    GameObject Time;
    GameObject Order;
    GameObject Score;
    GameObject OrderName;
    GameObject OrderSprite;
    GameObject Station1;
    GameObject Station2;
    GameObject Coin;
    GameObject Trash;
    GameObject VIP;

    private void Start()
    {
        print(Screen.currentResolution);
        Debug.Log(SceneID.id);
        //Testing
        PhotonNetwork.ConnectUsingSettings();
        //
        currentPosX = transform.position.x;
        Time = GameObject.Find("TimeUI");
        Order = GameObject.Find("OrderSystem");
        Station1 = GameObject.Find("Station1");
        Station2 = GameObject.Find("Station2");
        Trash = GameObject.Find("Trash");
        VIP = GameObject.Find("VIP");

        if (SceneID.id == 1){
            MoveToNewRoom(1);
            Time.GetComponent<PlayerReposition>().MoveToNewRoom(2);
            Order.GetComponent<PlayerReposition>().MoveToNewRoom(2);
            Station1.GetComponent<PlayerReposition>().MoveToNewRoom(2);
            Station2.GetComponent<PlayerReposition>().MoveToNewRoom(2);
            Trash.GetComponent<PlayerReposition>().MoveToNewRoom(2);
            VIP.GetComponent<PlayerReposition>().MoveToNewRoom(2);
        }
    }

    private void Update()
    {
        // Screen.SetResolution(w, h, false);
        if(firstTime)
        {
            transform.position = Vector3.SmoothDamp(transform.position, 
            new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);
            firstTime = false;
        }
    }

    public void MoveToNewRoom(int player)
    {
        currentPosX = currentPosX + 22 * player;
    }

    //For testing
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions {MaxPlayers = 4}, TypedLobby.Default);
    }
    // public override void OnJoinedRoom()
    // {
    //     Debug.Log("Joined");
    //     int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
    //     if (playerCount > 1){
    //         MoveToNewRoom(playerCount - 1);
    //         Order.GetComponent<PlayerReposition>().MoveToNewRoom(playerCount);
    //         Score.GetComponent<PlayerReposition>().MoveToNewRoom(playerCount);
    //         OrderName.GetComponent<PlayerReposition>().MoveToNewRoom(playerCount);
    //         OrderSprite.GetComponent<PlayerReposition>().MoveToNewRoom(playerCount);
    //         Station1.GetComponent<PlayerReposition>().MoveToNewRoom(playerCount);
    //         Station2.GetComponent<PlayerReposition>().MoveToNewRoom(playerCount);
    //     }
    // }
}