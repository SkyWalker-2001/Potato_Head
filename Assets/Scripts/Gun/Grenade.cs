using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Grenade : MonoBehaviour
{
    public Action OnExplode;

    [SerializeField] private GameObject _explodeVFX;
    [SerializeField] private float _launchForce = 15f;
    [SerializeField] private float _torqueAmount = 2f;

 
    private Rigidbody2D _rigidbody;
    private CinemachineImpulseSource _impulseSource;

    private void OnEnable()
    {
        OnExplode += Explosion;
        OnExplode += GrenadeScreenShake;
    }

    private void OnDisable()
    {
        OnExplode -= Explosion;
        OnExplode -= GrenadeScreenShake;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        LaunchGrenade();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            OnExplode?.Invoke();
        }
    }

    private void LaunchGrenade()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToMouse = (mousePos - (Vector2)transform.position).normalized;
        _rigidbody.AddForce(directionToMouse * _launchForce, ForceMode2D.Impulse);
        _rigidbody.AddTorque(_torqueAmount, ForceMode2D.Impulse);
    }

    private void Explosion()
    {
        Instantiate(_explodeVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void GrenadeScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }
}
 