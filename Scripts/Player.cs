using System;
using UnityEngine;

public class Player : Character
{

    [SerializeField] private GameObject weapon;

    private void Start()
    {
        Define();
    }
    

    private void FixedUpdate() {
        Controls();
    }

    private void Controls()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            Move(Input.GetAxisRaw("Horizontal"));
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Dash(Direction);
        }

        if (Input.GetMouseButtonDown(1))
        {
            weapon.GetComponent<Rifle>().Shoot();
        }
        
        WeaponControl();
    }
    
    // Similarity Theorem
    private void WeaponControl()
    {
        Vector2 pos = transform.position;
        Vector2 weaponPos = weapon.transform.position;
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector2 V, v; // Vectors of x and y diff.s
        float D, d; // Distances
        V = mouse - pos;
        v = weaponPos - pos;
        D = Vector2.Distance(mouse, pos);
        d = Vector2.Distance(weaponPos, pos);

        weaponPos = new Vector2(V.x * d / D, V.y * d / D) + pos;

        weapon.transform.position = weaponPos;

    }

    private void Dash(float x)
    {
        Body.AddForce(16f * new Vector2(x, 0), ForceMode2D.Impulse);
    }
}
