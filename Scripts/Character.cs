using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] protected float speed, jumpForce, maxFallHeight;
    protected Rigidbody2D Body;
    
    protected bool Grounded, Moving, Alive, Hurt;
    protected int _direction;
    [SerializeField] private Transform massCenter;
    
    [SerializeField] protected LayerMask groundLayer;

    private float _gravityDamage;

    protected Animator Animator;
    
    public int Direction {
        get { return _direction; }
    }

    private static class AnimationStates
    {
        public static readonly string 
            Idle = "Idle",
            Walk = "Walk",
            Die = "Die",
            Hurt = "Hurt";
    }

    private SpriteRenderer _sr;

    protected void CharacterStart()
    {
        Body = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _gravityDamage = 0;
        Alive = true;
        Hurt = false;
    }

    protected void CharacterFixedUpdate()
    {
        AnimatorController();
        TouchingGround();
        GravityDamage();
        Flip();
        Die();
    }
    
    protected void Move(int direction, float factor = 1)
    {
        Body.velocity = new Vector2(direction * speed, Body.velocity.y * factor); _direction = direction;
        Moving = true;
    }
    protected void Jump()
    {
        if (Grounded)
        {
            Grounded = false;
            Body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    public virtual void WeaponControl(){}

    public void Damage(float damage)
    {
        if (damage == 0) return;
        health -= damage;
        StartCoroutine(GetHurt());
    }

    private IEnumerator GetHurt()
    {
        Hurt = true;
        yield return new WaitForSeconds(0.4f);
        Hurt = false;
    }

    private void TouchingGround()
    {
        Grounded = Physics2D.Raycast(massCenter.position, new Vector2(0, -1), 0, groundLayer);
    }
    
    private void Die()
    {
        if (health <= 0)
            Alive = false;
        if (!Alive)
        {
            StartCoroutine(DelayDestroy(Animator.GetCurrentAnimatorStateInfo(0).length));
        }
    }

    private IEnumerator DelayDestroy(float s)
    {
        yield return new WaitForSeconds(s);
        Destroy(gameObject);
    }
    
    private void GravityDamage()
    {
        if (!Grounded)
        {
            var origin = massCenter.position;
            var direction = new Vector2(0, -1);
            var hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, groundLayer);
            
            Debug.DrawRay(origin, direction * hit.distance, Color.magenta);

            if (hit.distance > maxFallHeight)
            {
                _gravityDamage += Time.fixedDeltaTime;
            }
            
        }
        else
        {
            Damage(_gravityDamage);
            _gravityDamage = 0;
        }
    }

    protected void TriggerAnimation(string states)
    {
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName(states)) return;
        Animator.SetTrigger(states);
    }
    
    private void AnimatorController()
    {
        Animator.SetBool(AnimationStates.Walk, Moving);
        Animator.SetBool(AnimationStates.Hurt, Hurt);

        if (!Alive)
            TriggerAnimation(AnimationStates.Die);
    }

    private void Flip()
    {
        Quaternion look;
        var reset = Quaternion.Euler(0, 0,0);
        if (_direction == 1)
            look = Quaternion.Euler(0, 0, 0);
        else
            look = Quaternion.Euler(0, -180, 0);
        transform.rotation = Quaternion.Slerp(reset, look, 1f);
    }
    
}