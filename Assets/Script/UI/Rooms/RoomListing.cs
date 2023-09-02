using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    public RoomInfo RoomInfo { get; private set; }
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _text.text = roomInfo.Name;
    }
    public void OnClick_Button()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
        SceneID.roomName = RoomInfo.Name;
    }
}
