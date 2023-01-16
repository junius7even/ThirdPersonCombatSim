using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;

    private float health;
    private bool isInvulnerable;

    public event Action OnTakeDamage;

    public event Action OnDie;

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }

    private void Start()
    {
        health = maxHealth;

    }

    public void DealDamage(int damageAmount)
    {
        if (health < 0) { return; }
        if (isInvulnerable) { return; }

        health = Mathf.Max(health - damageAmount, 0);

        OnTakeDamage?.Invoke();

        if (health == 0)
        {
            OnDie?.Invoke(); // Invoke the onDie event on anyone who's subscribed to it
        }
        
        Debug.Log(health);
    }
}
