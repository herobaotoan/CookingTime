using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(Rigidbody2D))]

public class MouseDragHinge : Photon.Pun.MonoBehaviourPun, IPunOwnershipCallbacks
{
    //This script is used to pick up 2d rigid bodies and spin them/throw them
    public Rigidbody2D rb;

    GameObject HingePoint;
    HingeJoint2D hinge;

    Vector2 velocity;
    Vector2 lastPosition;
    Vector2 objPosition;

    public float maxVelocity = 0.5f;

    bool move = false;
    bool isFading = false;
    bool firstClick = true;

    RaycastHit hit;

    new AudioSource audio;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        //Finds a GameObject with the name "HingePoint"
        HingePoint = GameObject.Find("HingePoint");
        StartCoroutine(Fade());
        // Drag();
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse down");
        audio.Play();
        //Transfer ownership on click
        if(!photonView.IsMine) {
            base.photonView.TransferOwnership( PhotonNetwork.LocalPlayer.ActorNumber );
        }
        //Saves the mouse position to screen coordinates
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        HingePoint.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(mousePosition).x, Camera.main.ScreenToWorldPoint(mousePosition).y, HingePoint.transform.position.z);

        //Saves the HingeJoint2D component to variable that we can use
        hinge = HingePoint.GetComponent(typeof(HingeJoint2D)) as HingeJoint2D;
        
        move = true;
        hinge.enabled = true;
        //Assigns whatever rigid body we have clicked on to our hinge
        hinge.connectedBody = rb;
        //Prevents the hinge from adjusting the anchorpoint during fixed update, this well be set to true in OnMouseUp
        hinge.autoConfigureConnectedAnchor = false;
    }

    private void Update()
    {
        if (move == true)
        {
            //Saves the mouse position to screen coordinates
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            HingePoint.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(mousePosition).x, Camera.main.ScreenToWorldPoint(mousePosition).y, HingePoint.transform.position.z);
            
            objPosition = transform.position;
            //Calculates velocity
            velocity = (objPosition - lastPosition) / (Time.fixedDeltaTime);
            lastPosition = transform.position;

        }
    }

    private void OnMouseUp()
    {
        hinge.connectedBody = null;
        hinge.autoConfigureConnectedAnchor = false;
        hinge.enabled = false;
        move = false;
        //Combines the velocity from the mouse with the velocity from the hinge
        if ((rb.velocity+velocity).magnitude > new Vector2(maxVelocity, maxVelocity).magnitude)
        {
            // Debug.Log(velocity);
            // rb.velocity = new Vector2(maxVelocity*(Mathf.Abs(velocity.x)/velocity.x), maxVelocity*(Mathf.Abs(velocity.y)/velocity.y));
            rb.velocity = new Vector2(velocity.x/maxVelocity, velocity.y/maxVelocity);
        } else 
        {
            rb.velocity = rb.velocity+velocity;
        }
        StartCoroutine(Fade());

    }
    //Slowdown velocity
    IEnumerator Fade()
    {   
        if (!isFading)
        {
            isFading = true;
            while(rb.velocity.magnitude > 0)
            {
                yield return new WaitForSeconds(0.3f);
                rb.velocity = new Vector2(rb.velocity.x/1.5f, rb.velocity.y/1.5f);
            }
        isFading = false;
        }
    }
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Portal"))
        {
            Debug.Log("Hit Portal");
            photonView.RPC("SyncMovement", RpcTarget.All, transform.position, rb.velocity);
        //    Destroy(gameObject);
        }
    }

    [PunRPC]
    void SyncMovement(Vector3 newPosition, Vector2 newVelocity){
        TurnVisible();
        transform.position = newPosition;
        rb.velocity = newVelocity;
        StartCoroutine(Fade());
    }

    private void TurnVisible(){
        if (firstClick){
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
            firstClick = false;
        }
    }
}