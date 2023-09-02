using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OwnershipTransfer : MonoBehaviourPun, IPunOwnershipCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log("Request");
        if (targetView != base.photonView)
        return;

        base.photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("Transfered");
        if (targetView != base.photonView)
        return;

    }
    public void OnOwnershipTransferFailed(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("Failed");
        if (targetView != base.photonView)
        return;
    }

    void OnStart()
    {
        base.photonView.RequestOwnership();
    }
}
