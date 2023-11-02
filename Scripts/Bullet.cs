using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    private Rigidbody2D _body;
    [SerializeField] private float power;
    private Character _parentCharacter;

    private void Start()
    {
        _parentCharacter = transform.parent.parent.gameObject.GetComponent<Character>();
        _body = GetComponent<Rigidbody2D>();
        _body.velocity = new Vector2(power * _parentCharacter.Direction, _body.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        var target = other.gameObject.GetComponent<Character>();

        if (target)
        {
            target.Damage(damage);
        }
        
        Destroy(gameObject);
    }
}