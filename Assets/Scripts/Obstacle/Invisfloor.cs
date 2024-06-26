using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisfloor : MonoBehaviour
{
    public Transform targetWaypoint; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = targetWaypoint.position;
        }
    }
}
