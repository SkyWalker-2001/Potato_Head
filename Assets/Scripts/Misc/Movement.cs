using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;

    private float _moveX;
    private bool _canMove = true;

    private Rigidbody2D _rigidbody;
    private KnockBack _knockBack;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnEnable()
    {
        _knockBack.OnKnockbackStart += CanMoveFalse;
        _knockBack.OnKnockbackEnd += CanMoveTrue;
    }

    private void OnDisable()
    {
        _knockBack.OnKnockbackStart -= CanMoveFalse;
        _knockBack.OnKnockbackEnd -= CanMoveTrue;
    }

    public void SetCurrentDirection(float currentDirection)
    {
        _moveX = currentDirection;
    }

    private void CanMoveTrue()
    {
        _canMove = true;
    }

    private void CanMoveFalse()
    {
        _canMove = false;
    }

    private void Move()
    {
        if (!_canMove)
        {
            return;
        }

        Vector2 movement = new Vector2(_moveX * _moveSpeed, _rigidbody.velocity.y);
        _rigidbody.velocity = movement;
    }
}
