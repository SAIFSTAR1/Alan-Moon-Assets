using UnityEngine;

public class Player : MonoBehaviour
{

    private Character _character;
    [SerializeField] private GameObject weapon;
    
    private void Start()
    {
        _character = GetComponent<Character>();
    }

    
private void FixedUpdate() {
        Controls();
    }

    private void Controls()
    {
        Move();
        WeaponControl();
        
        if (Input.GetButtonDown("Jump"))
        {
            _character.Jump();
        }

        
    }
    
    private void Move()
    {
        _character.Move(Input.GetAxisRaw("Horizontal"));
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
    
    
}
