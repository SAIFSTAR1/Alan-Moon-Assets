using UnityEngine;


public class MeleeEnemy : EnemyBase
{
    [SerializeField] private Transform attackPoint;
    
    protected void MeleeEnemyStart()
    {
        EnemyBaseStart();
    }

    protected void MeleeEnemyUpdate()
    {
        EnemyBaseUpdate();
        Attack();
    }

    protected void MeleeEnemyFixedUpdate()
    {
        EnemyBaseFixedUpdate();
        
    }

    private void Attack()
    {
        Attacking = false;
        if (!CanAttack) return;

        Attacking = true;
        var player = Physics2D.OverlapCircle(attackPoint.position, attackRange/2, playerLayer);
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange/2);
    }
}