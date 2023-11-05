using System;
using UnityEngine;

namespace Enemies
{
    public class BugVil : MeleeAndShooterEnemy
    {

        private bool _flying;
        
        private void Start()
        {
            MSStart();
        }

        private void Update()
        {
            MSUpdate();
        }

        private void FixedUpdate()
        {
            MSFixedUpdate();
            AnimatorController();
            FlyToPlayer();
        }
        
        private void FlyToPlayer()
        {
            var direction = 0;
            _flying = false;

            if (Player.position.y - transform.position.y >= 0.4f)
                direction = 1;
            else if (Player.position.y - transform.position.y < -3f)
                direction = -1;
            else
                return;

            if (!Grounded) _flying = true;
            Body.velocity = new Vector2(Body.velocity.x,  direction * 3f);
        }

        private void AnimatorController()
        {
            Animator.SetBool("Flying", _flying);
        }
        
    }
}