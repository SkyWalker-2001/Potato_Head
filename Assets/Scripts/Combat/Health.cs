 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    public static Action<Health> OnDeath;

    public GameObject SplatterPrefab => _splatterPrefab;
    public GameObject DeathVFX => _deathVFX;

    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private GameObject _deathVFX;
    [SerializeField] private int _startingHealth = 3;

    private KnockBack _knockback;
    private Flash _flash;
    private Health _health;

    private int _currentHealth;


    private void Awake()
    {
        _knockback = GetComponent<KnockBack>();
        _flash = GetComponent<Flash>();
        _health = GetComponent<Health>();
    }

    private void Start() {
        ResetHealth();
    }

    public void ResetHealth() { 
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) 
        {
            OnDeath?.Invoke(this);

            Destroy(gameObject);
        }
    }

    public void TakeDamage(Vector2 damageSourceDir, int damageAmount, float knockBackThrust)
    {
        _health.TakeDamage(damageAmount);
        _knockback.GetKnockedBack(damageSourceDir, knockBackThrust);
    }

    public void TakeHit()
    {
        _flash.StartFlash();
    }

}
