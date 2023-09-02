using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PowerupProtection : MonoBehaviourPun
{
    public Button button;
    public Image image;
    public Sprite normalSprite;
    public Sprite usingSprite;
    public new AudioSource audio;
    public int uses;
    public int price;
    bool isActive = false;

    public float TimeLeft;
    GameObject board1;
    GameObject board2;
    GameObject pan1;
    GameObject pan2;
    GameObject order;

    [SerializeField]
    Image coinImage;
    [SerializeField]
    TextMeshProUGUI priceTag;
    
    
    void Start()
    {
        board1 = GameObject.Find("Board");
        board2 = GameObject.Find("Board1");
        pan1 = GameObject.Find("Pan0");
        pan2 = GameObject.Find("Pan1");
        order = GameObject.Find("Order");
    }

    //Update Time
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
                SetBonus(false);
                TimeLeft = 30;
                isActive = false;
                button.interactable = true;
            }
        }

        //Update Price Tag
        if (uses > 0)
        {
            changeTag(uses);
        } else {
            changeTag(price);
        }
    }
    
    public void Activate()
    {
        if (!isActive)
        {
            photonView.RPC("PunActivate", RpcTarget.All);
        }
    }

    [PunRPC]
    void PunActivate()
    {
        //If there is free use
        if (uses > 0)
        {
            audio.Play();
            image.sprite = usingSprite;
            SetBonus(true);
            isActive = true;
            button.interactable = false;
            uses -= 1;
        //Cost money
        } else {
            bool checkCoin = order.GetComponent<OrderSystem>().UseCoin(price);
            if (checkCoin)
            {
                audio.Play();
                image.sprite = usingSprite;
                SetBonus(true);
                isActive = true;
                button.interactable = false;
            } else {
                StartCoroutine("TurnRed");
            }
        }
    }
    
    //Not enough money
    IEnumerator TurnRed()
    {
        image.color = Color.red;
        yield return new WaitForSeconds(1f);
        image.color = Color.white;
    }
    
    void SetBonus(bool state)
    {
        board1.GetComponent<ChangeSprite>().SetIsProtected(state);
        board2.GetComponent<ChangeSprite>().SetIsProtected(state);
        pan1.GetComponent<ChangeSprite>().SetIsProtected(state);
        pan2.GetComponent<ChangeSprite>().SetIsProtected(state);
    }

    void changeTag(int num)
    {
        if (num == price)
        {
            priceTag.text = num.ToString();
            coinImage.gameObject.SetActive(true);
        } else 
        {
            priceTag.text = "0" + num.ToString();
            coinImage.gameObject.SetActive(false);
        }
    }
    
    public void GiveFreeUse()
    {
        photonView.RPC("PunGiveFreeUse", RpcTarget.All);
    }

    [PunRPC]
    void PunGiveFreeUse()
    {
        uses += 1;
    }
}
