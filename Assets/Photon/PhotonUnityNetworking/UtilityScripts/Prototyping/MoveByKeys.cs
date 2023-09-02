using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Photon.Pun.UtilityScripts
{
    [RequireComponent(typeof(PhotonView))]
    public class MoveByKeys : Photon.Pun.MonoBehaviourPun, IPunOwnershipCallbacks
    {
        public float Speed = 10f;
        public float JumpForce = 200f;
        public float JumpTimeout = 0.5f;

        private bool isSprite;
        private float jumpingTime;
        private Rigidbody body;
        private Rigidbody2D body2d;
        public MonoBehaviourPun[] scripsToIgnore;

        public void Start()
        {
            Debug.Log(photonView.IsMine);
            // if(!photonView.IsMine) {
            //     Destroy(gameObject);
            // }
            this.isSprite = (GetComponent<SpriteRenderer>() != null);

            this.body2d = GetComponent<Rigidbody2D>();
            this.body = GetComponent<Rigidbody>();
        }
        void OnMouseDown()
        {
            Debug.Log("Click");
            if(!photonView.IsMine) {
                base.photonView.TransferOwnership( PhotonNetwork.LocalPlayer.ActorNumber );
            }
        }
        // Update is called once per frame
        public void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                base.photonView.TransferOwnership( PhotonNetwork.LocalPlayer.ActorNumber );
                // return;
            }

            if ((Input.GetAxisRaw("Horizontal") < -0.1f) || (Input.GetAxisRaw("Horizontal") > 0.1f))
            {
                
                if(!photonView.IsMine) {
                    base.photonView.TransferOwnership( PhotonNetwork.LocalPlayer.ActorNumber );
                }
                transform.position += Vector3.right * (Speed * Time.deltaTime) * Input.GetAxisRaw("Horizontal");
            }

            // jumping has a simple "cooldown" time but you could also jump in the air
            if (this.jumpingTime <= 0.0f)
            {
                if (this.body != null || this.body2d != null)
                {
                    // obj has a Rigidbody and can jump (AddForce)
                    if (Input.GetKey(KeyCode.Space))
                    {
                        this.jumpingTime = this.JumpTimeout;

                        Vector2 jump = Vector2.up * this.JumpForce;
                        if (this.body2d != null)
                        {
                            this.body2d.AddForce(jump);
                        }
                        else if (this.body != null)
                        {
                            this.body.AddForce(jump);
                        }
                    }
                }
            }
            else
            {
                this.jumpingTime -= Time.deltaTime;
            }

            // 2d objects can't be moved in 3d "forward"
            if (!this.isSprite)
            {
                if ((Input.GetAxisRaw("Vertical") < -0.1f) || (Input.GetAxisRaw("Vertical") > 0.1f))
                {
                    transform.position += Vector3.forward * (Speed * Time.deltaTime) * Input.GetAxisRaw("Vertical");
                }
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
    }
}