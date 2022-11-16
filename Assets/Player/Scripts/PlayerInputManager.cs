using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInputManager : MonoBehaviour
{
    PlayerAttack _playerAttack;
    PlayerInput _playerInput;

    InputActionMap _currentActionMap;
    InputAction _moveAction;
    InputAction _lookAction;
    InputAction _fireAction;
    InputAction _aimAction;

    public Vector2 MoveValue { get; private set; }
    public Vector2 LookValue { get; private set; }
    public bool IsAiming { get; private set; }

    private void Awake()
    {
        _playerAttack = GetComponent<PlayerAttack>();
        _playerInput = GetComponent<PlayerInput>();

        _currentActionMap = _playerInput.currentActionMap;

        _moveAction = _currentActionMap.FindAction("Move");
        _lookAction = _currentActionMap.FindAction("Look");
        _aimAction = _currentActionMap.FindAction("Aim");
        _fireAction = _currentActionMap.FindAction("Fire");

        _moveAction.performed += OnMove;
        _lookAction.performed += OnLook;
        _aimAction.started += OnMouse1Pressed;
        _fireAction.started += OnMouse0Pressed;

        _moveAction.canceled += OnMove;
        _lookAction.canceled += OnLook;
        _aimAction.canceled += OnMouse1Pressed;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        _currentActionMap.Enable();
    }

    private void OnDisable()
    {
        _currentActionMap.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        MoveValue = context.ReadValue<Vector2>();
    }

    void OnLook(InputAction.CallbackContext context)
    {
        LookValue = context.ReadValue<Vector2>();
    }

    void OnMouse0Pressed(InputAction.CallbackContext context)
    {
        _playerAttack.Fire();
    }

    void OnMouse1Pressed(InputAction.CallbackContext context)
    {
        IsAiming = context.ReadValueAsButton();
    }
}
