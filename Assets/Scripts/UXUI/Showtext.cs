using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Showtext : MonoBehaviour
{
    public TextMeshProUGUI textElement; 
    public Transform player; 
    public float fadeThreshold = 10.0f; 
   // public float destroyThreshold = 15.0f; 

    void Update()
    {
        float distance = Vector3.Distance(textElement.transform.position, player.transform.position);

        float fadeFactor = Mathf.Clamp01(distance / fadeThreshold);
        textElement.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, fadeFactor));

        // if (distance > destroyThreshold)
        // {
        //     Destroy(gameObject);
        // }
    }
}
