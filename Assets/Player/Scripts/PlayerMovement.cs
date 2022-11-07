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

    Camera _mainCamera;
    Rigidbody _rb;

    float _xRotationInput;
    float _yRotationInput;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        RotateHead();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void RotateHead()
    {
        float xAxis = Input.GetAxis("Mouse X");
        float yAxis = Input.GetAxis("Mouse Y");

        float xValue = xAxis * _mouseSensivity * Time.deltaTime;
        float yValue = yAxis * _mouseSensivity * Time.deltaTime;

        _xRotationInput += xValue;
        _xRotationInput = Mathf.Clamp(_xRotationInput, -_maxHorizontalLookAngle, _maxHorizontalLookAngle); //to dont let player to rotate wrongly

        _yRotationInput += yValue;
        _yRotationInput = Mathf.Clamp(_yRotationInput, -_maxVerticalLookAngle, _maxVerticalLookAngle); //to dont let player to rotate wrongly

        _mainCamera.transform.localRotation = Quaternion.Euler(-_yRotationInput, _xRotationInput, 0);
    }

    void Move()
    {
        //getting input //may need some improvement on horizontal a,d movement
        float forwardMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal");

        //if we click any movement key correct body
        if (forwardMovement != 0 || horizontalMovement != 0)
        {
            CorrectBodyRotation();
        }

        //for equalize value of rotationinput with localrotation after moving//may need some improvement. Sometimes it flips all the way
        if (_mainCamera.transform.localRotation.y < 0)
        {
            _xRotationInput = _mainCamera.transform.localEulerAngles.y - 360;
        }
        else
        {
            _xRotationInput = _mainCamera.transform.localEulerAngles.y;
        }

        //calculating velocities depending on camera's transform
        Vector3 zVelocity = forwardMovement * _forwardMovementSpeed * _mainCamera.transform.forward;
        Vector3 horizontalVelocity = horizontalMovement * _sideMovementSpeed * _mainCamera.transform.right;

        //to dont fly
        Vector3 finalVelocity = new Vector3(zVelocity.x + horizontalVelocity.x, _rb.velocity.y, zVelocity.z + horizontalVelocity.z);

        _rb.velocity = finalVelocity;
    }

    void CorrectBodyRotation()
    {
        //getting rotation to do same with camera's forward
        Quaternion lookRotation = Quaternion.LookRotation(_mainCamera.transform.forward);
        lookRotation.x = 0f;
        lookRotation.z = 0f;

        //to left same rotation on camera
        Vector3 cameraForward = _mainCamera.transform.forward;

        //rotating body
        transform.localRotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.05f);

        //to left same rotation on camera
        _mainCamera.transform.forward = cameraForward;
    }
}
