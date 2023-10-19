using UnityEngine;

public class Character : MonoBehaviour
{
    
    [SerializeField]
    private float health;
    [SerializeField]
    private float speed, jumpForce;
    private Rigidbody2D _body;

    private bool _grounded;

    [SerializeField]
    private LayerMask GroundLayer;

    private void Start()
    {
        _grounded = true;
        _body = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Die();
    }

    public void Move(float x)
    {
        transform.position += speed * Time.deltaTime * new Vector3(x, 0, 0);
    }

    public void Jump()
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _grounded = true;
        }
    }
}