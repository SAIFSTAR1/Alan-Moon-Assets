using System;
using UnityEngine;

public class EnemyBase : Character
{

    private bool _attack, _atTheEdge;
    [SerializeField] private float attackRange, chaseRange, maxFallHeight;
    private Transform _player;
    private float _playerDistance;
    private float _edgeDistance;

    [SerializeField] private Transform rFoot, lFoot;

    public bool attack
    {
        get { return _attack; }
    }
    
    private void Start()
    {
        Define();
        _atTheEdge = false;

        try
        {
            _player = GameObject.FindWithTag("Player").transform;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
        
    }

    private void FixedUpdate()
    {
        
        _playerDistance = Vector3.Distance(transform.position, _player.position);
        
        ChangeDirection();
        
        if (!_atTheEdge)
            FollowPlayer();
        CheckEdges();
    }

    protected void Attack()
    {
        if ( _playerDistance <= attackRange )
            _attack = true;
        else
            _attack = false;
        
    }

    private void FollowPlayer()
    {
        if (_player)
            if ( _playerDistance <= chaseRange && _playerDistance >= attackRange )
                if (transform.position.x <= _player.position.x)
                    Move(1);
                else
                    Move(-1);
            
    }

    private void CheckEdges()
    {
        
        RaycastHit2D hit;
        Vector2 origin, direction = new Vector2(0, -1);
            
        if (_direction == 1)
            origin = rFoot.position;
        else
            origin = lFoot.position;
        
        hit = Physics2D.Raycast(origin, direction, groundLayer);
        Debug.DrawRay(origin, direction * hit.distance, Color.red);

        if (hit.distance > maxFallHeight || !hit)
            _atTheEdge = true;
        else
            _atTheEdge = false;

    }

    private void ChangeDirection()
    {
        if (_playerDistance <= chaseRange)
        {
            if (transform.position.x <= _player.position.x)
                _direction = 1;
            else
                _direction = -1;
        }
    }
}