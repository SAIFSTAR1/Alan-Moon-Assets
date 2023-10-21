using UnityEngine;

public class Character : MonoBehaviour
{
    
    [SerializeField]
    private float health;
    [SerializeField]
    private float speed, jumpForce;
    private Rigidbody2D _body;
    private bool _grounded;
    private float _direction;

    [SerializeField]
    private LayerMask GroundLayer;

    protected Rigidbody2D Body
    {
        get { return _body; }
    }

    protected float Direction
    {
        get { return _direction; }
    }

    protected void Define()
    {
        _body = GetComponent<Rigidbody2D>();
    }
    
    protected void Move(float x)
    {
        var v = speed;
        if (!_grounded)
            v /= 1.6f;
        _body.velocity = new Vector2(x * v, _body.velocity.y);

        _direction = x;
    }

    public void Test()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _grounded = true;
        }
    }
}