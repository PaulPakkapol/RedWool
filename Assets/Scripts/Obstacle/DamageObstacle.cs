using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObstacle : MonoBehaviour
{
    public int damageAmount = 10;

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag("Player"))
        {
            if (col.GetComponent<PlayerHealth>() != null)
            {
                col.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            }
            
        }
        else
        {
            Debug.LogError("Player object missing PlayerHealth component!");
        }
       

    }
}
    

