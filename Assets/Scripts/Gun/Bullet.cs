using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockBackThrust = 20;

    [SerializeField] private GameObject _bulletVFX;

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

        Instantiate(_bulletVFX, transform.position, Quaternion.identity);

        IHitable iHitable = other.gameObject.GetComponent<IHitable>();
        iHitable?.TakeHit();
            
        IDamageable iDamageable = other.gameObject.GetComponent<IDamageable>();
        iDamageable?.TakeDamage(_fireDirection, _damageAmount, _knockBackThrust);

        _gun.ReleaseBulletFromPool(this);
    }
}