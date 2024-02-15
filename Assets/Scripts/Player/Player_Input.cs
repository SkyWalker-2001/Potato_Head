using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Input : MonoBehaviour
{
    private PlayerInputAction _playerInputAction;
    private InputAction _move;
    private InputAction _jump;

    private void Awake()
    {
        _playerInputAction = new PlayerInputAction();

        _move = _playerInputAction.Player.Move;
        _jump = _playerInputAction.Player.Jump;

    }

    private void OnEnable()
    {
        _playerInputAction.Enable();
    }

    private void OnDisable()
    {
        _playerInputAction?.Disable();
    }

    private void Update()
    {
        
    }
}
