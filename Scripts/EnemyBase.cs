using System;
using UnityEngine;

public class EnemyBase : Character
{

    private bool _attack;
    [SerializeField] private float attackRange, chaseRange;
    private Transform _player;
    private float _playerDistance;
    
    private void Start()
    {
        Define();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        FollowPlayer();
    }

    protected void Attack()
    {
        _playerDistance = Vector3.Distance(transform.position, _player.position);
        
        if ( _playerDistance <= attackRange )
            _attack = true;
        else
            _attack = false;
        
    }

    protected void FollowPlayer()
    {
        _playerDistance = Vector3.Distance(transform.position, _player.position);
        
        if ( _playerDistance <= chaseRange && _playerDistance >= attackRange )
            if (transform.position.x <= _player.position.x)
                Move(1);
            else
                Move(-1);
            
    }
    
}