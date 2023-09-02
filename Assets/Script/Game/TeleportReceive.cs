using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class TeleportReceive : MonoBehaviour, IOnEventCallback
{
    public byte TELEPORT_EVENT = 0;

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData obj)
    {
        if (obj.Code == TELEPORT_EVENT)
        {
            object[] data = (object[])obj.CustomData;
            Vector2 velocity = (Vector2)data[0];
            Vector3 position = (Vector3)data[1];

            GameObject clone = (GameObject)PhotonNetwork.Instantiate("Square", new Vector2(position.x +1, position.y), Quaternion.identity);
            // clone.GetComponent<Rigidbody2D>().velocity = velocity;
        }
    }
}
