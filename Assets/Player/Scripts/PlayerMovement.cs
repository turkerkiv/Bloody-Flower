using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _forwardMovementSpeed = 5f;
    [SerializeField] float _sideMovementSpeed = 5f;

    [SerializeField] float _mouseSensivity = 60f;
    [SerializeField] float _maxHorizontalLookAngle = 65f;
    [SerializeField] float _maxVerticalLookAngle = 65f;

    PlayerAttack _playerAttack;
    Camera _mainCamera;
    Rigidbody _rb;
    Animator _animator;

    float _xRotationInput;
    float _yRotationInput;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
        _playerAttack = GetComponent<PlayerAttack>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Rotate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Rotate()
    {
        float xAxis = Input.GetAxis("Mouse X");
        float yAxis = Input.GetAxis("Mouse Y");

        float xValue = xAxis * _mouseSensivity * Time.deltaTime;
        float yValue = yAxis * _mouseSensivity * Time.deltaTime;

        _xRotationInput += xValue;

        _yRotationInput += yValue;
        _yRotationInput = Mathf.Clamp(_yRotationInput, -_maxVerticalLookAngle, _maxVerticalLookAngle); //to dont let player to rotate wrongly

        if (!_playerAttack.IsAiming)
        {
            _mainCamera.transform.localRotation = Quaternion.Euler(-_yRotationInput, -15f, 0);
        }
        else
        {
            _mainCamera.transform.localRotation = Quaternion.Slerp(_mainCamera.transform.localRotation, Quaternion.Euler(0, -15f, 0), 0.05f);
            _yRotationInput = 0;
        }
        transform.rotation = Quaternion.Euler(0, _xRotationInput, 0);
    }

    void Move()
    {
        //getting input //may need some improvement on horizontal a,d movement
        float forwardMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal");

        bool isWalking = forwardMovement != 0 ? true : false;
        _animator.SetBool("_isWalking", isWalking);

        //calculating velocities depending on camera's transform
        Vector3 zVelocity = forwardMovement * _forwardMovementSpeed * _mainCamera.transform.forward;
        Vector3 horizontalVelocity = horizontalMovement * _sideMovementSpeed * _mainCamera.transform.right;

        //to not fly
        Vector3 finalVelocity = new Vector3(zVelocity.x + horizontalVelocity.x, _rb.velocity.y, zVelocity.z + horizontalVelocity.z);

        _rb.velocity = finalVelocity;
    }
}
