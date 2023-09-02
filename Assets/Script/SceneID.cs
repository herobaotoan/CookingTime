using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SceneID : MonoBehaviourPun
{
    public static int id;

    //Default time is 5m
    public static int time = 300;

    public static string roomName;

    [SerializeField]
    Button button5m;
    [SerializeField]
    Button button10m;
    [SerializeField]
    TextMeshProUGUI IDText1;
    [SerializeField]
    TextMeshProUGUI IDText2;

    void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            id = 0;
        } else { 
            IDText2.text = "P1";
            IDText1.text = "P2";
            id = 1; 
        }
    }

    public void OnClickSwitchID()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            photonView.RPC("PunSwitchID", RpcTarget.All);
        }
    }

    public void OnClickSwitchTime(int newTime)
    {
        photonView.RPC("PunSwitchTime", RpcTarget.All, newTime);
    }

    [PunRPC]
    public void PunSwitchID()
    {
        if (id == 1)
        {
            IDText2.text = "P2";
            IDText1.text = "P1";
            id = 0;
        } else { 
            IDText1.text = "P2";
            IDText2.text = "P1";
            id = 1; 
        }
    }

    [PunRPC]
    public void PunSwitchTime(int newTime)
    {
        time = newTime;
        if (newTime == 300)
        {
            button5m.interactable = false;
            button10m.interactable = true;
        } else {
            button5m.interactable = true;
            button10m.interactable = false;
        }
    }
}
