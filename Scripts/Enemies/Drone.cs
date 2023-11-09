using System;
using UnityEngine;

namespace Enemies
{
    public class Drone : MeleeEnemy
    {
        private void Start()
        {
            MeleeEnemyStart();
        }

        private void Update()
        {
            MeleeEnemyUpdate();
            SetAttackState(new Vector2(_direction, -1));
        }

        private void FixedUpdate()
        {
            MeleeEnemyFixedUpdate();
            FlyToPlayer();
        }

        private void FlyToPlayer()
        {
            var direction = 0;

            if (Player.position.y - transform.position.y >= 0)
                direction = 1;
            else if (Player.position.y - transform.position.y < -1.4f)
                direction = -1;
            
            Body.velocity = new Vector2(Body.velocity.x,  direction * 3f);
        }

        protected override void FollowPlayer()
        {

            Moving = false;
            
            if (!Player) return;
            if (!(PlayerDistance <= chaseRange)) return;
            if (!(PlayerDistance >= attackRange)) return;

            Moving = true;
            
            if (transform.position.x <= Player.position.x)
                Move(1);
            else
                Move(-1);
        }

    }
}