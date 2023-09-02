using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun; 

public class CreateIngredient : Photon.Pun.MonoBehaviourPun
{
    // public byte CREATE_EVENT = 0;

    // private void OnMouseDown()
    // {
    //     Debug.Log("Clicked");
    //     object[] content = new object[] { };
    //     RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; 
    //     PhotonNetwork.RaiseEvent(CREATE_EVENT, content, raiseEventOptions, SendOptions.SendReliable);
    // }

    // private void OnEnable()
    // {
    //     PhotonNetwork.AddCallbackTarget(this);
    // }

    // private void OnDisable()
    // {
    //     PhotonNetwork.RemoveCallbackTarget(this);
    // }

    // public void OnEvent(EventData obj)
    // {
    //     if (obj.Code == CREATE_EVENT)
    //     {
    //         GameObject clone = (GameObject)PhotonNetwork.Instantiate("Square", transform.position, Quaternion.identity);
    //     }
    // // }
    // public GameObject myPrefab;
    public string ingredient;

    // private void OnTriggerExit2D(Collider2D col)
    // {
    //     if (col.CompareTag("Meat(new)")){
    //         photonView.RPC("SpawnMeat", RpcTarget.All);
    //     }
    // }

    public void PunSpawnIngredient(){
        photonView.RPC("SpawnIngredient", RpcTarget.Others);
    }

    [PunRPC]
    void SpawnIngredient()
    {
        // Instantiate(myPrefab, transform.position, Quaternion.identity);
        GameObject clone = (GameObject)PhotonNetwork.Instantiate(ingredient, transform.position, Quaternion.identity);
    }
}
