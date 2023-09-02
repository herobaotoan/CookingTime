using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerCountdown : MonoBehaviour
{
    public float TimeLeft;
    public bool TimerOn = false;

    private int SelectedTime;
    private int ID;

    public TextMeshPro TimerTxt;
    public GameObject canvas;
    public GameObject order;
    public GameObject VIP;

    private int SelectedTimeLeft;
   
    void Start()
    {
        SelectedTime = SceneID.time;
        ID = SceneID.id;
        TimeLeft = SelectedTime + 3;
        //If 5m, VIP order appears once every 1m
        if (SelectedTime == 300)
        {
            SelectedTimeLeft = SelectedTime - 60 - (ID * 60);
        } else {
        //If 10m, VIP appears once every 2m
            SelectedTimeLeft = SelectedTime - 120 - (ID * 120);
        }
        canvas = GameObject.Find("Canvas");
        order = GameObject.Find("Order");
        TimerOn = true;
    }

    void Update()
    {
        if(TimerOn)
        {
            if(TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
                //Update Bonus rate through time
                if (TimeLeft > SelectedTime - (SelectedTime / 5) - 0.1 && TimeLeft < SelectedTime - (SelectedTime / 5) + 0.1)
                {
                    Debug.Log("Bonus 1");
                    order.GetComponent<OrderSystem>().SetBonusState(1);
                }
                if (TimeLeft > (SelectedTime / 5) - 0.1 && TimeLeft < (SelectedTime / 5) + 0.1)
                {
                    Debug.Log("Bonus 2");
                    order.GetComponent<OrderSystem>().SetBonusState(2);
                }
                //VIP Orders
                if (TimeLeft > SelectedTimeLeft - 0.1 && TimeLeft < SelectedTimeLeft + 0.1)
                {
                    //If 5m, VIP order appears once every 1m
                    if (SelectedTime == 300)
                    {
                        SelectedTimeLeft -= 120;
                    } else {
                    //If 10m, VIP appears once every 2m
                        SelectedTimeLeft -= 240;
                    }
                    VIP.GetComponent<VIPTimer>().SetTimerOn();
                }
            }
            else
            {
                canvas.GetComponent<ScoreBoard>().Show();
                TimeLeft = 0;
                TimerOn = false;
            }
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}