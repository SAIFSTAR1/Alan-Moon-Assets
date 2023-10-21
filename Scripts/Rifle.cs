using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private int ammo;

    public int Ammo
    {
        set { ammo = value; }
    }

    public void Shoot()
    {
        if (ammo > 0)
        {
            Instantiate(bullet, transform);
            ammo--;
        }
    }
}
