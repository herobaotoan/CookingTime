using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text _roomName;
    [SerializeField]
    GameObject text;

    private RoomsCanvases _roomsCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    public void OnClick_CreateRoom()
    {
        if (_roomName.text != "")
        {
            if (!PhotonNetwork.IsConnected)
                return;
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;
            options.IsVisible = true;
            PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);
            SceneID.roomName = _roomName.text;
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room Successfully.", this);
        _roomsCanvases.CurrentRoomCanvas.Show();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        text.SetActive(true);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        text.SetActive(true);
    }
}
