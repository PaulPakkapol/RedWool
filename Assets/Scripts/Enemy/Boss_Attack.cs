using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Attack : MonoBehaviour
{
    public int attackDamage = 20;
    public int enragedAttackDamage = 40;

    public Vector3 attackOffset;
    public float attackRange;
    public LayerMask attackMask;
    public Transform bossAttackPoint;

    public void Attack()
    {
       // Vector3 pos = transform.position;
       // pos += transform.right * attackOffset.x;
       //pos += transform.up * attackOffset.y;

        Collider2D[] colInfo = Physics2D.OverlapCircleAll(bossAttackPoint.position, attackRange, attackMask);
        foreach (Collider2D  player in colInfo)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage); 
        }
        
    }
    
    private void OnDrawGizmosSelected()
    {
        if (bossAttackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(bossAttackPoint.position,attackRange);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
