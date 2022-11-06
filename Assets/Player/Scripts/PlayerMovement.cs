using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _forwardMovementSpeed = 5f;
    [SerializeField] float _sideMovementSpeed = 5f;

    [SerializeField] float _mouseSensivity = 60f;

    Camera _mainCamera;
    Rigidbody _rb;

    float _xRotationInput;
    float _yRotationInput;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
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
        _yRotationInput += yValue;

        _mainCamera.transform.rotation = Quaternion.Euler(-_yRotationInput, _xRotationInput, 0);
    }

    void Move()
    {
        //needs some improvement on horizontal a,d movement

        //getting input
        float forwardMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal");

        //if we click any movement key correct body
        if (forwardMovement != 0 || horizontalMovement != 0)
        {
            CorrectBodyRotation();
        }

        //calculating velocities depending on camera's transform
        Vector3 zVelocity = forwardMovement * _forwardMovementSpeed * _mainCamera.transform.forward;
        Vector3 horizontalVelocity = horizontalMovement * _sideMovementSpeed * _mainCamera.transform.right;

        //to dont fly
        Vector3 finalVelocity = new Vector3(zVelocity.x + horizontalVelocity.x, 0f, zVelocity.z + horizontalVelocity.z);

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
