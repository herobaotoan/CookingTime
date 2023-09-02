using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun; 

public class Teleport : MonoBehaviour
{
    public byte TELEPORT_EVENT = 0;
    // void OnCollisionEnter2D(Collision2D col)
    // {
    //     Debug.Log("Hit");
        
    //     if (col.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("Hit");
    //         object[] content = new object[] { col.relativeVelocity, col.gameObject.transform.position };
    //         RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
    //         PhotonNetwork.RaiseEvent(TELEPORT_EVENT, content, raiseEventOptions, SendOptions.SendReliable);
    //         Destroy(col.gameObject);
    //     }
    // }
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Hit");
        
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Portal");
            object[] content = new object[] { col.gameObject.GetComponent<Rigidbody2D>().velocity, col.gameObject.transform.position };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
            PhotonNetwork.RaiseEvent(TELEPORT_EVENT, content, raiseEventOptions, SendOptions.SendReliable);
            Destroy(col.gameObject);
        }
    }
}
