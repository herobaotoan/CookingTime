using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OrderSystem : MonoBehaviourPun
{
    public int score;
    public int orderNum;
    public string[] nameList;
    public float[] stateList;
    public int[] scoreList;
    public Sprite[] spriteList;

    public bool bonus;
    public int bonusState;
    int previousBonusState;

    public int currentOrder;

    public GameObject meal;
    public GameObject scoreText;
    public GameObject spriteOrder;

    public new SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        orderNum = 0;
        bonus = false;
        bonusState = 0;
        previousBonusState = 0;
        renderer = GetComponent<SpriteRenderer>();
        ChangeOrder(-1);
        //Sync order to other player
        if (SceneID.id == 1){
            photonView.RPC("ChangeOrder", RpcTarget.All, currentOrder);
        }
    }

    void Update()
    {
        if (meal != null)
        {
            if (CheckOrder())
            {   
                UnsetOrder();
                //Sync Score and next Order
                photonView.RPC("UpdateScore", RpcTarget.All, 0);
                ChangeOrder(-1);
                photonView.RPC("ChangeOrder", RpcTarget.Others, currentOrder);
            } else {
                UnsetOrder();
                Debug.Log("Wrong");
            }
        }
    }

    public bool CheckOrder()
    {
        if (meal.name.Contains(nameList[currentOrder]) && stateList[currentOrder] == meal.GetComponent<ChangeState>().GetState())
        {
            return true;
        } else {
            return false;
        }
    }
    
    [PunRPC]
    public void ChangeOrder(int number)
    {
        if (Random.Range(0, bonusState * 3) == 1)
        {
            bonus = true;
        } else {
            bonus = false;
        }
        if (bonusState == 2)
        {
            bonus = true;
        }
        if (bonus)
        {
            //Change Bonus Sprite
            renderer.color = Color.yellow;
        } else {
            renderer.color = Color.white;
        }
        if (number >= 0)
        {
            currentOrder = number;
        } else {
            currentOrder = Random.Range(0, nameList.Length);
        }
        Debug.Log("Current Order: " + currentOrder);
        //Display order sprite
        spriteOrder.GetComponent<ChangeOrderSprite>().ChangeSprite(spriteList[currentOrder]);
    }
    
    public void PunChangeOrder()
    {
        photonView.RPC("ChangeOrder", RpcTarget.Others, currentOrder);
    }

    public void PunUpdateScore(int updateScore)
    {
        photonView.RPC("UpdateScore", RpcTarget.All, updateScore);
    }

    [PunRPC]
    void UpdateScore(int updateScore)
    {
        if (updateScore == 0)
        {
            if (bonus)
            {
                score += (scoreList[currentOrder] + scoreList[currentOrder]/2);
            } else {
                score += scoreList[currentOrder];
            }
            orderNum += 1;
            scoreText.GetComponent<ChangeText>().Change(score.ToString());
        } else {
            //Use coin or VIP get coin
            score += updateScore;
            scoreText.GetComponent<ChangeText>().Change(score.ToString());
        }
    }

    public bool UseCoin(int coin)
    {
        if (score >= coin)
        {
            photonView.RPC("UpdateScore", RpcTarget.All, -coin);
            return true;
        } else {
            return false;
        }
    }

    public void SetOrder(GameObject obj)
    {
        meal = obj;
    }
    void UnsetOrder()
    {
        meal = null;
    }

    public void SetBonusState(int state)
    {
        //-1 is to reset bonus back to normal
        if (state == -1)
        {
            bonusState = previousBonusState;
        } else if (state < bonusState) //Prevent time affecting powerup
        {
            previousBonusState = state;
        } else
        {
            previousBonusState = bonusState;
            bonusState = state;
        }
        Debug.Log("Bonus: " + bonusState);
    }

    public int GetScore()
    {
        return score;
    }
    public int GetOrderNum()
    {
        return orderNum;
    }
}
