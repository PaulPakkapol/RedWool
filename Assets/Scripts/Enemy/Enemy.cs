using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    private int currentHealth;
    public Healthbar healthbar;
    //public bool isDead;
    
    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        healthbar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Debug.Log("Enemie die");
        animator.SetBool("IsDead",true);
        //isDead = true;
        GetComponent<Collider2D>().enabled = false;
        
       // if(isDead = true)
       // {
       //     Task.Delay(3000);
       //     if (enabled)
       //     {
       //         Destroy(gameObject);
       //     }
       //      
       // }
    }
}
