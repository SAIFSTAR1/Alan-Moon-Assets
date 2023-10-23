using UnityEngine;

public class Character : MonoBehaviour
{
    
    [SerializeField]
    private float health;
    [SerializeField]
    private float speed, jumpForce;
    public Rigidbody2D _body;
    private bool _grounded;
    public int _direction;

    [SerializeField] protected LayerMask groundLayer;


    protected void Define()
    {
        _body = GetComponent<Rigidbody2D>();
    }
    
    protected void Move(int direction)
    {
        _body.velocity = new Vector2(direction * speed, _body.velocity.y);
        _direction = direction;
    }
    protected void Jump()
    {
        if (_grounded)
        {
            _grounded = false;
            _body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
    }

    private void Die()
    {
        if (health <= 0)
            Destroy(gameObject);
    }
}