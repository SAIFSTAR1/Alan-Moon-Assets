﻿using UnityEngine;

namespace Enemies
{
    public class MiniRobot : ShooterEnemy
    {
        private void Start()
        {
            ShooterEnemyStart();
        }

        private void Update()
        {
            ShooterEnemyUpdate();
            SetAttackState(new Vector2(_direction, 0));
        }

        private void FixedUpdate()
        {
            ShooterEnemyFixedUpdate();
        }
    }
}