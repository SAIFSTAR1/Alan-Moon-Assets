using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    private Rigidbody2D _body;
    

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
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