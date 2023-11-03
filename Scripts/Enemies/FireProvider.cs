using System;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public class FireProvider : ShooterEnemy
    {
        [SerializeField] private GameObject bazooka;
        private Rifle _bazookaRifle;

        private enum Directions
        {
            Forward,
            Up,
            Down,
            None
        }

        private Directions _weaponDir;

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
            var origin = bazooka.transform.position;
            var flat = Physics2D.Raycast(origin, new Vector2(_direction, 0), Mathf.Infinity, playerLayer);
            var posInclined = Physics2D.Raycast(origin, new Vector2(_direction, 1), Mathf.Infinity, playerLayer);
            var negInclined = Physics2D.Raycast(origin, new Vector2(_direction, -1), Mathf.Infinity, playerLayer);

            if (flat)
            {
                Debug.DrawRay(origin, new Vector2(_direction, 0), Color.green);
                _weaponDir = Directions.Forward;
            }

            else if (posInclined)
            {
                Debug.DrawRay(origin, new Vector2(_direction, 1), Color.green);
                _weaponDir = Directions.Up;
            }
            
            else if (negInclined)
            {
                Debug.DrawRay(origin, new Vector2(_direction, -1), Color.green);
                _weaponDir = Directions.Down;
            }
            else
            {
                _weaponDir = Directions.None;
            }
        }

        private void AnimatorController()
        {
            Animator.SetTrigger(_weaponDir.ToString());
        }
    }
}