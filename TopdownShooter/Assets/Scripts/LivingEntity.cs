using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;
    public float health;
    protected bool dead;
    public Stat healthBar;
    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
        healthBar.Initialize();
        healthBar.MaxValue = health;
        healthBar.CurrentValue = health;
    }

    public void TakeDamage(float damage, RaycastHit hit)
    {
        health -= damage;
        healthBar.CurrentValue = health;
        if (health <= 0)
            Die();
    }

    /// <summary>
    /// Invoke ondeath event.
    /// Increase score if enemy died.
    /// </summary>
    protected void Die()
    {
        if (OnDeath != null)
        {
            OnDeath();
        }
        Enemy enemy = gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            GameManager.Instance.score = GameManager.Instance.score + enemy.pointsAwardedOnDeath;
            GameManager.Instance.scoreText.text = "Score-> " + GameManager.Instance.score.ToString();
            if(GameManager.Instance.highScore < GameManager.Instance.score)
            {
                PlayerPrefs.SetInt("HighScore", GameManager.Instance.score);
            }
        }
        gameObject.SetActive(false);
        gameObject.transform.parent = ObjectPooler.Instance.PoolParent;
    }
}
