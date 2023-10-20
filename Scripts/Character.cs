using UnityEngine;

public class Character : MonoBehaviour
{
    
    [SerializeField]
    private float health;
    [SerializeField]
    private float speed, jumpForce;
    private Rigidbody2D _body;

    private bool _grounded;

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