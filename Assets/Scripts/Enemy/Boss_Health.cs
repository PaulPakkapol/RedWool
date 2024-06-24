using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Health : MonoBehaviour
{
    public int health = 300;
    public Animator animator;
    public Healthbar healthbar;
    

   //public GameObject deathEffect;

    public bool isInvulnerable = false;

    public void Start()
    {
        healthbar.SetMaxHealth(health);
    }

    public void TakeDamage(int damage)
    {
         if (isInvulnerable)
            return;
         
        health -= damage;
        healthbar.SetHealth(health);

        if (health <= 150)
        {
            GetComponent<Animator>().SetBool("IsEnraged", true);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        animator.SetBool("IsDead",true);
        //isDead = true;
        //GetComponent<Collider2D>().enabled = false;
        //Destroy(gameObject);
    }
}
