using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveRoomMenu : MonoBehaviour
{

    private RoomsCanvases _roomsCanvases;
    public GameObject PlayerListingMenu;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }
    public void OnClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        PlayerListingMenu.GetComponent<PlayerListingMenu>().OnLeftRoom();
        _roomsCanvases.CurrentRoomCanvas.Hide();
    }
}
