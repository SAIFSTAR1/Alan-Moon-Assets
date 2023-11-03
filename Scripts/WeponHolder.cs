using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeponHolder : MonoBehaviour
{
    int CurrentWepone = 1;
    int NumOfWepons;
    public Transform[] Weapons;

    void Start()
    {
        Weapons = GetComponentsInChildren<Transform>();
        NumOfWepons = Weapons.Length - 1;
        CurrentWepone = 1;
        ChangeWeapon(CurrentWepone);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(2);
        }
    }

    void ChangeWeapon(int x)
    {
        CurrentWepone = x;
            
        for (int i = 1; i <= NumOfWepons; i++)
        {
            if (i == CurrentWepone)
            {
                Weapons[i].gameObject.SetActive(true);
            }
            else
            {
                Weapons[i].gameObject.SetActive(false);
            }
        }
    }

}
