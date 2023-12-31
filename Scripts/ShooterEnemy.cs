﻿using UnityEngine;

public class ShooterEnemy : EnemyBase
{
    [SerializeField] protected GameObject weapon;
    private Rifle _rifle;
    [SerializeField] private float fireRate;
    private float _nextFire;
    
    protected void ShooterEnemyStart()
    {
        EnemyBaseStart();
        _rifle = weapon.GetComponent<Rifle>();
    }

    protected void ShooterEnemyUpdate()
    {
        EnemyBaseUpdate();
        Attack();
    }

    protected void ShooterEnemyFixedUpdate()
    {
        EnemyBaseFixedUpdate();
    }

    private void Attack()
    {
        Attacking = false;
        if (!CanAttack) return;
        
        if (_nextFire <= 0)
        {
            Attacking = true;
            _rifle.Invoke("Attack", 0.52f);
            _nextFire = fireRate;
        }
        else
        {
            _nextFire -= Time.deltaTime;
        }
    }
}