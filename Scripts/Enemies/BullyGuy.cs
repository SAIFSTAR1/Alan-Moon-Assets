using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    public class BullyGuy : MeleeEnemy
    {
        private void Start()
        {
            MeleeEnemyStart();
        }

        private void Update()
        {
            MeleeEnemyUpdate();
            SetAttackState(new Vector2(_direction, 0));
        }

        private void FixedUpdate()
        {
            MeleeEnemyFixedUpdate();
        }
    }
}

