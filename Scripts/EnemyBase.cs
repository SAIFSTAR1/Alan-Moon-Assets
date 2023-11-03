using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBase : Character
{
    
    protected bool CanAttack, Attacking, AtTheEdge;
    [SerializeField] protected float attackRange, chaseRange, stopRange, decelerationPoint;
    protected Transform Player;
    protected float PlayerDistance;
    private float _edgeDistance;

    [SerializeField] private Transform edgeCheck;
    [SerializeField] protected LayerMask playerLayer;
    private static class AnimationStates
    {
        public static readonly string
            Attack = "Attack";
    }
    
    protected void EnemyBaseStart()
    {
        CharacterStart();
        AtTheEdge = false;

        try
        {
            Player = GameObject.FindWithTag("Player").transform;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
        
    }

    protected void EnemyBaseUpdate()
    {
        
    }
    
    protected void EnemyBaseFixedUpdate()
    {
        AnimatorController();
        CharacterFixedUpdate();
        PlayerDistance = Vector3.Distance(transform.position, Player.position);
        ChangeDirection();
        FollowPlayer();
        CheckEdges();
    }

    protected virtual void SetAttackState(Vector2 direction)
    {
        var origin = transform.position;
        bool hit = Physics2D.OverlapCircle(origin, attackRange, playerLayer);

        if (hit)
        {
            Debug.Log("Detected");
            CanAttack = true;
        }
        else
        {
            CanAttack = false;
            Debug.Log("Not Detected");
        }
        
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected virtual void FollowPlayer()
    {
        Moving = false;
        
        if (!Player) return;
        if (!(PlayerDistance <= chaseRange)) return;
        if (!(PlayerDistance >= stopRange)) return;
        if (AtTheEdge) return;

        Moving = true;
        var diff = transform.position.x - Player.position.x;
        if (Math.Abs(diff) <= decelerationPoint)
        {
            if (diff < 0)
                Move(1, 0.1f);
            else
                Move(-1, 0.1f);
            return;
        }
        if (transform.position.x <= Player.position.x)
            Move(1);
        else
            Move(-1);
    }

    private void CheckEdges()
    {
        
        RaycastHit2D hit;
        Vector2 origin = edgeCheck.position, direction = new Vector2(0, -1);
        
        hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, groundLayer);
        Debug.DrawRay(origin, direction * hit.distance, Color.red);

        if (hit.distance > maxFallHeight || !hit)
            AtTheEdge = true;
        else
            AtTheEdge = false;

    }

    protected virtual void ChangeDirection()
    {
        if (!(PlayerDistance <= chaseRange)) return;
        
        if (transform.position.x <= Player.position.x)
            _direction = 1;
        else if (transform.position.x > Player.position.x)
            _direction = -1;
        
    }

    private void AnimatorController()
    {
        if (Attacking)
            TriggerAnimation(AnimationStates.Attack);
    }
    
}