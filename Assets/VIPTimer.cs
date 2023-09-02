using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VIPTimer : MonoBehaviour
{
    public float TimeLeft;
    public bool TimerOn = false;
    
    [SerializeField]
    TextMeshPro TimerTxt;
    [SerializeField]
    GameObject VIP;

    void Update()
    {
        if(TimerOn)
        {
            if(TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                MoveOut();
                TimeLeft = 0;
                TimerOn = false;
            }
        }
    }

    
    void updateTimer(float currentTime)
    {
        currentTime += 1;
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = seconds.ToString() + "s";
    }

    public void SetTimerOn()
    {
        TimerOn = true;
        TimeLeft = 30;
        StartCoroutine("Move", 3.5f);
    }

    public void MoveOut()
    {
        Debug.Log("Moveout");
        TimerOn = false;
        TimeLeft = 0;
        StartCoroutine("Move", -3.5f);
    }

    IEnumerator Move(float direction)
    {
        float timeSinceStarted = 0f;
        Vector3 destination = new Vector3(VIP.transform.position.x, VIP.transform.position.y + direction, VIP.transform.position.z);
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            VIP.transform.position = Vector3.Lerp(VIP.transform.position, destination, timeSinceStarted);

            //If the object has arrived, stop the coroutine
            if (VIP.transform.position == destination)
            {
                Debug.Log("Done moving");
                yield break;
            }

            //Otherwise, continue next frame
            yield return null;
        }
    }
}
