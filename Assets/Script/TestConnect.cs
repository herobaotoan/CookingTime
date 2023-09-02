using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TestConnect : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Button createRoomButton;
    
    private void Start()
    {
        createRoomButton.gameObject.SetActive(false);
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            print("Connecting to server");
            PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
            PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        } else {
            createRoomButton.gameObject.SetActive(true);
        }

    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon.", this);
        Debug.Log("My nickname is " + PhotonNetwork.LocalPlayer.NickName, this);
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();

        createRoomButton.gameObject.SetActive(true);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected to Photon for reason" + cause.ToString());
    }
}
