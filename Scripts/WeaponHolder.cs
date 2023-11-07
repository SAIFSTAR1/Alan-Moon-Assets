using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponHolder : MonoBehaviour
{
    private int CurrentWeapon;
    private int _numOfWeapons;
    public Weapon[] weapons;
    private Player _player;
    private const string WeaponSelected = "WeaponSelected";
    public Weapon currentWeapon;

    private void Start()
    {
        _player = transform.parent.GetComponent<Player>();
        weapons = GetComponentsInChildren<Weapon>();
        _numOfWeapons = weapons.Length - 1;
        CurrentWeapon = 0;
        ChangeWeapon(CurrentWeapon);
    }
    
    private void Update()
    {
        SetAnimationState();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(1);
        }

        currentWeapon = weapons[CurrentWeapon];
    }

    private void ChangeWeapon(int x)
    {
        CurrentWeapon = x;
            
        for (int i = 1; i <= _numOfWeapons; i++)
        {
            if (i == CurrentWeapon)
            {
                weapons[i].gameObject.SetActive(true);
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }

    private void SetAnimationState()
    {
        _player.animator.SetInteger(WeaponSelected, CurrentWeapon);
    }

}
