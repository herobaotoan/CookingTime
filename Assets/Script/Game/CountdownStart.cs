using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CountdownStart : MonoBehaviourPun
{
    public bool isReady = false;
    public float TimeLeft;
    public bool TimerOn = false;

    public TextMeshProUGUI TimerTxt;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        // Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady)
        {
            photonView.RPC("SetReady", RpcTarget.Others);
        }
        if (isReady)
        {
            photonView.RPC("StartCountdown", RpcTarget.All);
        }
        if(TimerOn)
        {
            if(TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                gameObject.SetActive(false);
                TimeLeft = 0;
                TimerOn = false;
            }
        }
    }

    [PunRPC]
    public void SetReady()
    {
        isReady = true;
    }

    [PunRPC]
    public void StartCountdown()
    {
        isReady = true;
        Time.timeScale = 1;
        TimerOn = true;
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0}", seconds);
    }
}
