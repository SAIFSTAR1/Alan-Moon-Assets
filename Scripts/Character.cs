using UnityEngine;

public class Character : MonoBehaviour
{
    
    [SerializeField]
    private float health;
    [SerializeField]
    private float speed, jumpForce;
    public Rigidbody2D _body;
    private bool _grounded;
    public float _direction;

    [SerializeField]
    private LayerMask GroundLayer;


    protected void Define()
    {
        _body = GetComponent<Rigidbody2D>();
    }
    
    protected void Move(float direction)
    {
        _body.velocity = new Vector2(direction * speed, _body.velocity.y);
    }
    protected void Jump()
    {
            if (_grounded)
            {
                _grounded = false;
                _body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
    }

    private void Die()
    {
        if (health <= 0)
            Destroy(gameObject);
    }
}