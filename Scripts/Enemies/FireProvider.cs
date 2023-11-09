using System;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public class FireProvider : ShooterEnemy
    {

        private void Start()
        {
            ShooterEnemyStart();
        }

        private void Update()
        {
            ShooterEnemyUpdate();
            SetAttackState(new Vector2(_direction, 0));
            WeaponControl();
        }

        private void FixedUpdate()
        {
            ShooterEnemyFixedUpdate();
            AnimatorController();
        }

        public override void WeaponControl()
        {
            
        }

        private void WeaponPower()
        {
            var powerRatio = Math.Abs(Player.position.x - transform.position.x);
        }

        private void AnimatorController()
        {
        }
    }
}