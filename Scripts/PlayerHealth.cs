using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int CurrentHealth;
    public int MaxHealth;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void Damage(int DamageAmount)
    {
        CurrentHealth -= DamageAmount;
        if (CurrentHealth >= 0)
        {
            Die();
        }
    }

    void Die()
    {
        
    }

}
