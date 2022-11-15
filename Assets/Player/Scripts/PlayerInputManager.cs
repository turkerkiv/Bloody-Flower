using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInputManager : MonoBehaviour
{
    PlayerInput _playerInput;

    InputActionMap _currentActionMap;
    InputAction _moveAction;
    InputAction _lookAction;

    public Vector2 MoveValue { get; private set; }
    public Vector2 LookValue { get; private set; }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _currentActionMap = _playerInput.currentActionMap;
        _moveAction = _currentActionMap.FindAction("Move");
        _lookAction = _currentActionMap.FindAction("Look");

        _moveAction.performed += OnMove;
        _lookAction.performed += OnLook;

        _moveAction.canceled += OnMove;
        _lookAction.canceled += OnLook;
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
}
