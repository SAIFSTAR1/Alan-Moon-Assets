using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    private Rigidbody2D _body;
    [SerializeField] private float power;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.velocity = new Vector2(power, _body.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}