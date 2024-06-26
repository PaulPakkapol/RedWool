using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitwithTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Application.Quit();
        }
    }
}
