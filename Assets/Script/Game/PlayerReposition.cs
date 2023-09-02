using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReposition : MonoBehaviour
{
    private float currentPosX;


    public void MoveToNewRoom(int player)
    {
        currentPosX = (transform.position.x + 22) * (player-1);
        transform.position = new Vector3(currentPosX, transform.position.y, transform.position.z);
    }
}
