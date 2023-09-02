using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    GameObject order;

    public UnityEvent<string, int, string> submitScoreEvent;
    public void OnEnable()
    {
        if (SceneID.id == 0)
        {
            submitScoreEvent.Invoke(SceneID.roomName, order.GetComponent<OrderSystem>().GetOrderNum(), order.GetComponent<OrderSystem>().GetScore().ToString());
        }
    }
 
}
