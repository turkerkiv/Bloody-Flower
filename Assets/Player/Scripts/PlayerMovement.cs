using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform _cameraRoot;
    [SerializeField] float _forwardMovementSpeed = 5f;
    [SerializeField] float _sideMovementSpeed = 5f;

    [SerializeField] float _mouseSensivity = 10f;
    [SerializeField] float _maxVerticalLookAngle = 65f;

    Camera _mainCamera;
    Rigidbody _rb;
    Animator _animator;
    Player _player;

    float _yRotationInput;
    Vector2 _currentVelocity;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _player.PlayerInputManager.HideCursor();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        Rotate();
    }

    void Rotate()
    {
        _mainCamera.transform.position = _cameraRoot.position;

        float xAxis = _player.PlayerInputManager.LookValue.x;
        float yAxis = _player.PlayerInputManager.LookValue.y;

        float xValue = xAxis * _mouseSensivity * Time.deltaTime;
        float yValue = yAxis * _mouseSensivity * Time.deltaTime;

        _yRotationInput -= yValue;
        _yRotationInput = Mathf.Clamp(_yRotationInput, -_maxVerticalLookAngle, _maxVerticalLookAngle); //to dont let player to rotate wrongly

        if (!_player.PlayerInputManager.IsAiming)
        {
            _mainCamera.transform.localRotation = Quaternion.Euler(_yRotationInput, 0, 0);
        }
        else
        {
            _mainCamera.transform.localRotation = Quaternion.Slerp(_mainCamera.transform.localRotation, Quaternion.Euler(0, 0, 0), 0.05f); //NEED DELTATIME
            _yRotationInput = 0;
        }

        transform.Rotate(Vector3.up, xValue);
    }

    void Move()
    {
        _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _player.PlayerInputManager.MoveValue.x * _sideMovementSpeed, 9f * Time.fixedDeltaTime);
        _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _player.PlayerInputManager.MoveValue.y * _forwardMovementSpeed, 9f * Time.fixedDeltaTime);

        float xVelDifference = _currentVelocity.x - _rb.velocity.x;
        float zVelDifference = _currentVelocity.y - _rb.velocity.z;

        //transform Vector didnt get it
        _rb.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);

        _animator.SetFloat("_xVelocity", _currentVelocity.x);
        _animator.SetFloat("_zVelocity", _currentVelocity.y);
    }
}
