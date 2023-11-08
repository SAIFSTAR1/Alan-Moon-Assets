using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private int ammo;
    [SerializeField] private float power;
    private Character _holder;

    private void Start()
    {
        _holder = transform.parent.gameObject.GetComponent<Character>();
    }
    
    public int Ammo
    {
        set { ammo = value; }
    }

    public void Attack()
    {
        if (ammo <= 0) return;
        var angle = transform.rotation.z;
        var createdBullet = Instantiate(bullet, transform);
        var xPower = Convert.ToSingle(Math.Cos(angle)) * _holder.Direction;
        var yPower = Convert.ToSingle(Math.Sin(angle));
        createdBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(xPower, yPower) * power;
        ammo--;
    }
}
