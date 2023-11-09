using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int CurrentHealth;
    public int MaxHealth;
    private Player _player;
    void Start()
    {
        CurrentHealth = MaxHealth;
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void Damage(int DamageAmount)
    {
        CurrentHealth -= DamageAmount;
        if (CurrentHealth <= 0)
        {
            _player.PlayAnimationState(Player.AnimationStates.Die);
        }
    }

    private void Die()
    {
        Destroy(_player.gameObject);
    }

}
