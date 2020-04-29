using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;
    protected float health;
    protected bool dead;
    public Stat healthBar;
    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
        healthBar.Initialize();
    }

    public void TakeDamage(float damage, RaycastHit hit)
    {
        health -= damage;
        healthBar.CurrentValue = health;
        if (health <= 0)
            Die();
    }

    protected void Die()
    {
        if (OnDeath != null)
        {
            OnDeath();
        }
        gameObject.SetActive(false);
        gameObject.transform.parent = ObjectPooler.Instance.PoolParent;
    }
}
