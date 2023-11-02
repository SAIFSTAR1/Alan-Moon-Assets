using UnityEngine;


public class MeleeAndShooterEnemy : EnemyBase
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject weapon;
    private Rifle _rifle;
    [SerializeField] private float fireRate;
    private float _nextFire;
    private bool _melee, _fire;
    [SerializeField] private float meleeRange;

    protected void MSStart()
    {
        EnemyBaseStart();
        _rifle = weapon.GetComponent<Rifle>();
    }

    protected void MSUpdate()
    {
        EnemyBaseUpdate();
        SetAttackState(new Vector2(_direction, 0));
        Attack();
    }

    protected void MSFixedUpdate()
    {
        EnemyBaseFixedUpdate();
        AnimatorController();
    }

    private static class AnimationStates
    {
        public static readonly string
            Melee = "Melee",
            Fire = "Fire";
    }

    private void Attack()
    {
        Attacking = false;
        if (!CanAttack) return;
        
        var hit = Physics2D.Raycast(transform.position, new Vector2(_direction, 0), Mathf.Infinity, playerLayer);
        Debug.DrawRay(transform.position, new Vector2(_direction, 0) * hit.distance, Color.yellow);
        if (!hit) return;
        Attacking = true;
        
        if (hit.distance <= meleeRange)
        {
            MeleeAttack();
        }
        else
        {
            FireAttack();
        }
        
    }

    private void MeleeAttack()
    {
        _melee = true;
        _fire = false;
        var player = Physics2D.OverlapCircle(attackPoint.position, attackRange/2, playerLayer);
    }

    private void FireAttack()
    {
        _fire = true;
        _melee = false;
        if (_nextFire <= 0)
        {
            Attacking = true;
            _rifle.Shoot();
            _nextFire = fireRate;
        }
        else
        {
            _nextFire -= Time.deltaTime;
        }
    }
    private void AnimatorController()
    {
        if (_melee)
            Animator.SetTrigger(AnimationStates.Melee);
        if (_fire)
            Animator.SetTrigger(AnimationStates.Fire);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, meleeRange);
    }
}