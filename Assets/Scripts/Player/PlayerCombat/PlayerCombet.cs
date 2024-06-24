using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerCombet : MonoBehaviour
{

    public LayerMask enemylayers;
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayerAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        
    }

    void PlayerAttack()
    {
        animator.SetTrigger("Attack");

        // Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemylayers);
        // foreach (Collider2D enemy in hitEnemies)
        // {
        //     enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        // }
        
        Collider2D hitEnemies = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemylayers);
         if (hitEnemies != null && hitEnemies.GetComponent<Boss_Health>() != null )
         {
             hitEnemies.GetComponent<Boss_Health>().TakeDamage(attackDamage);
         }
         if (hitEnemies != null && hitEnemies.GetComponent<Enemy>() != null)
        {
            hitEnemies.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
}
