using System;
using UnityEngine;

public class EnemyBase : Character
{

    private bool _attack, _atTheEdge;
    [SerializeField] private float attackRange, chaseRange;
    private Transform _player;
    private float _playerDistance;
    private float _edgeDistance;

    [SerializeField] private Transform edgeCheck;
    
    private void Start()
    {
        Define();
        _atTheEdge = false;
        _player = GameObject.FindWithTag("Player").transform;
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
        if ( _playerDistance <= chaseRange && _playerDistance >= attackRange )
            if (transform.position.x <= _player.position.x)
                Move(1);
            else
                Move(-1);
            
    }

    private void CheckEdges()
    {
        var hit = Physics2D.Raycast(edgeCheck.position, new Vector2(_direction, 0));
        Debug.DrawRay(edgeCheck.position, new Vector2(_direction, 0) * hit.distance, Color.red);

        if (hit.distance <= 1f)
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