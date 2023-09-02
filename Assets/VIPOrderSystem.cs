using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VIPOrderSystem : MonoBehaviourPun
{
    public int orderNum;
    public string[] nameList;
    public int[] scoreList;
    public Sprite[] spriteList;

    public int currentOrder;

    public GameObject meal;
    public GameObject VIPSpriteOrder;
    public GameObject VIPTimer;

    public GameObject ProtectionPU;
    public GameObject SpeedPU;

    private GameObject order;

    // Start is called before the first frame update
    void Start()
    {
        ChangeOrder();
        orderNum = 0;
        order = GameObject.Find("Order");
    }

    void Update()
    {
        if (meal != null)
        {
            if (CheckOrder())
            {   
                UnsetOrder();
                order.GetComponent<OrderSystem>().PunUpdateScore(scoreList[currentOrder]);
                GiveRandomPowerUp();
                VIPTimer.GetComponent<VIPTimer>().MoveOut();
                ChangeOrder();
            } else {
                UnsetOrder();
                VIPTimer.GetComponent<VIPTimer>().MoveOut();
                ChangeOrder();
            }
        }
    }

    public bool CheckOrder()
    {
        if (meal.name.Contains(nameList[currentOrder]))
        {
            return true;
        } else {
            return false;
        }
    }

    public void ChangeOrder()
    {
        currentOrder = Random.Range(0, nameList.Length);
        //Display order sprite
        VIPSpriteOrder.GetComponent<ChangeOrderSprite>().ChangeSprite(spriteList[currentOrder]);
    }
    
    public void SetOrder(GameObject obj)
    {
        meal = obj;
    }

    void UnsetOrder()
    {
        meal = null;
    }

    void GiveRandomPowerUp()
    {
        //50 / 50 chance for each powerup
        if (Random.Range(0,2) == 0)
        {
            ProtectionPU.GetComponent<PowerupProtection>().GiveFreeUse();
        } else {
            SpeedPU.GetComponent<PowerupFastProcess>().GiveFreeUse();
        }
    }
}
