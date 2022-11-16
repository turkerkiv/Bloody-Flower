using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInputManager : MonoBehaviour
{
    Player _player;
    
    PlayerInput _playerInput;

    InputActionMap _currentActionMap;
    InputAction _moveAction;
    InputAction _lookAction;
    InputAction _fireAction;
    InputAction _aimAction;
    InputAction _glassStateAction;

    public Vector2 MoveValue { get; private set; }
    public Vector2 LookValue { get; private set; }
    public bool IsAiming { get; private set; }

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerInput = GetComponent<PlayerInput>();

        _currentActionMap = _playerInput.currentActionMap;

        _moveAction = _currentActionMap.FindAction("Move");
        _lookAction = _currentActionMap.FindAction("Look");
        _aimAction = _currentActionMap.FindAction("Aim");
        _fireAction = _currentActionMap.FindAction("Fire");
        _glassStateAction = _currentActionMap.FindAction("ChangeGlassState");

        _moveAction.performed += OnMove;
        _lookAction.performed += OnLook;
        _aimAction.started += OnAim;
        _fireAction.started += OnFire;
        _glassStateAction.started += OnChangeGlassState;

        _moveAction.canceled += OnMove;
        _lookAction.canceled += OnLook;
        _aimAction.canceled += OnAim;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DisableAiming()
    {
        IsAiming = false;
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

    void OnFire(InputAction.CallbackContext context)
    {
        if (!IsAiming) { return; }
        _player.PlayerAttack.Fire(); // maybe change here to add ifisnotaimingreturn code here
    }

    void OnAim(InputAction.CallbackContext context)
    {
        if (_player.PlayerGlassManager.CurrentGlassState == PlayerGlassManager.GlassState.GlassOn) { return; }

        IsAiming = context.ReadValueAsButton();
    }

    void OnChangeGlassState(InputAction.CallbackContext context)
    {
        _player.PlayerGlassManager.ChangeGlassState();
    }
}
