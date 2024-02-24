using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Cinemachine;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;

    public static Action OnGrenadeShoot;

    [SerializeField] private Transform _bulletSpawnPoint;

    [Header("Bullet")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCD = .5f;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private float _muzzleFlashTime = .05f;

    [Header("Grenade")]
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private float _grenadeShootCD = .8f;

    private Coroutine _muzzleFlashRoutine;
    private ObjectPool<Bullet> _bulletPool;
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Vector2 _mousPos;
    private float _lastFireTime = 0f;
    private float _lastGrenadeTime = 0f;

    private Player_Input _playerInput;
    private FrameInput _frameInput;
    private CinemachineImpulseSource _impulseSource;
    private Animator _animator;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _animator = GetComponent<Animator>();
        _playerInput = GetComponentInParent<Player_Input>();
        _frameInput = _playerInput.FrameInput;
    }

    private void Start()
    {
        CreateBulletPool();
    }

    private void Update()
    {
        GatherInput();
        Shoot();
        RotateGun();
    }

    private void OnEnable()
    {
        OnShoot += FireAnimation;
        OnShoot += ShootProjectile;
        OnShoot += ResetLastFireTime;
        OnShoot += GunScreenShake;
        OnShoot += MuzzleFlash;
        OnGrenadeShoot += ShootGrenade;
        OnGrenadeShoot += FireAnimation;
        OnGrenadeShoot += ResetLastGrenadeTime;
    }

    private void OnDisable()
    {
        OnShoot -= FireAnimation;
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
        OnShoot -= GunScreenShake;
        OnShoot -= MuzzleFlash;
        OnGrenadeShoot -= ShootGrenade;
        OnGrenadeShoot -= FireAnimation;
        OnGrenadeShoot -= ResetLastGrenadeTime;
    }

    public void ReleaseBulletFromPool(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    private void GatherInput()
    {
        _frameInput = _playerInput.FrameInput;
    }

    private void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>( () => {
            return Instantiate(_bulletPrefab);
        }, bullet => {
            bullet.gameObject.SetActive(true);
        }, bullet => {
            bullet.gameObject.SetActive(false);
        }, bullet => {
            Destroy(bullet);
        }, false, 20 ,80);
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= _lastFireTime)
        {
            OnShoot.Invoke();
        } 

        if(_frameInput.Grenade && Time.time >= _lastGrenadeTime)
        {
            OnGrenadeShoot?.Invoke();
        }
    }

    private void ResetLastFireTime()
    {
        _lastFireTime = Time.time + _gunFireCD;
    }  
    
    private void ResetLastGrenadeTime()
    {
        _lastGrenadeTime = Time.time + _grenadeShootCD;
    }


    private void ShootProjectile()
    {
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, _bulletSpawnPoint.position, _mousPos);
    }

    private void ShootGrenade()
    {
        Instantiate(_grenadePrefab, _bulletSpawnPoint.position, Quaternion.identity);
        _lastGrenadeTime = Time.time;
    }

    private void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0, 0f);
    }

    private void GunScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    private void RotateGun()
    {
        _mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousPos);
        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0,0,angle);
    }

    private void MuzzleFlash()
    {
        if(_muzzleFlashRoutine != null)
        {
            StopCoroutine(_muzzleFlashRoutine);
        }

        _muzzleFlashRoutine = StartCoroutine(MuzzleFlashRoutine());
    }

    private IEnumerator MuzzleFlashRoutine()
    {
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_muzzleFlashTime);
        _muzzleFlash.SetActive(false);
    }
}
