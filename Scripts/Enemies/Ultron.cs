using System;
using UnityEngine;

namespace Enemies
{
    public class Ultron : MeleeAndShooterEnemy
    {
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
        }
    }
}