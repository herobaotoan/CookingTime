using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PowerUpsGoldenOrder : MonoBehaviourPun
{
    public Button button;
    public Image image;
    public int uses;
    bool isActive = false;
    public Sprite normalSprite;
    public Sprite usingSprite;
    public new AudioSource audio;

    public float TimeLeft;
    public GameObject order;
    
    [SerializeField]
    TextMeshProUGUI priceTag;
    
    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            if(TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
            }
            else
            {
                image.sprite = normalSprite;
                order.GetComponent<OrderSystem>().SetBonusState(-1);
                TimeLeft = 20;
                isActive = false;
                button.interactable = true;
            }
        }

        priceTag.text = "0" + uses.ToString();
        if (uses == 0)
        {
            button.interactable = false;
        } else {
            button.interactable = true;
        }
    }

    public void Activate()
    {
        photonView.RPC("PunActivate", RpcTarget.All);
    }

    [PunRPC]
    void PunActivate()
    {
        if (uses > 0)
        {   
            audio.Play();
            image.sprite = usingSprite;
            button.interactable = false;
            isActive = true;
            order.GetComponent<OrderSystem>().SetBonusState(2);
            if (SceneID.id == 1){
                order.GetComponent<OrderSystem>().ChangeOrder(-1);
                order.GetComponent<OrderSystem>().PunChangeOrder();
            }
        }
        uses -= 1;
    }
}
