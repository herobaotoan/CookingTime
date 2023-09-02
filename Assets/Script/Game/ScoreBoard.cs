using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public static bool TimesUp = false;
    public GameObject menuUI;
    int score;
    int orderNum;

    // Update is called once per frame
    public void Show()
    {
        menuUI.SetActive(true);
        Time.timeScale = 0f;
        TimesUp = true;
        GameObject order = GameObject.Find("Order");
        score = order.GetComponent<OrderSystem>().GetScore();
        orderNum = order.GetComponent<OrderSystem>().GetOrderNum();
        GameObject.Find("OrderServed").GetComponent<ChangeText>().Change("Order Served: " + orderNum);
        GameObject.Find("CoinsLeft").GetComponent<ChangeText>().Change("Coins: " + score);
    }
}
