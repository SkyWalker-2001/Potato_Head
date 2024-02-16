using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockBackThrust = 20;

    private Vector2 _fireDirection;
    private Gun _gun;

    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Init(Gun gun, Vector2 bulletSpawnPoint, Vector2 mousePos)
    {
        _gun = gun;
        transform.position = bulletSpawnPoint;
        _fireDirection = (mousePos - bulletSpawnPoint).normalized;
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Health health = other.gameObject.GetComponent<Health>();
        health?.TakeDamage(_damageAmount);

        KnockBack knockBack = other.gameObject.GetComponent<KnockBack>();
        knockBack?.GetKnockedBack(PlayerController.Instance.transform.position, _knockBackThrust);

        Flash flash = other.gameObject.GetComponent<Flash>();
        flash?.StartFlash();

        _gun.ReleaseBulletFromPool(this);
    }
}