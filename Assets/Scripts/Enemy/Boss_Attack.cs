using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Attack : MonoBehaviour
{
    public int attackDamage = 20;
    public int enragedAttackDamage = 40;

    public Vector3 attackOffset;
    public float attackRange =2f;
    public LayerMask attackMask;

    public void Update()
    {
        
    }

    public void BossAttack()
    {
       Vector3 pos = transform.position;
       pos += transform.right * attackOffset.x;
       pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null && colInfo.GetComponent<PlayerHealth>() != null)
        {
            colInfo.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }

    }
    
    public void BossEnragedAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null && colInfo.GetComponent<PlayerHealth>() != null)
        {
            colInfo.GetComponent<PlayerHealth>().TakeDamage(enragedAttackDamage);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        Gizmos.DrawWireSphere(pos,attackRange);
    }
}
