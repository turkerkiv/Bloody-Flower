using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject _crosshair;
    [SerializeField] Transform _tipOfWeapon;

    bool _isAiming;

    void Awake()
    {
    }

    void Update()
    {
        Aim();

        Fire();
    }

    void Aim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _isAiming = true;
            _crosshair.SetActive(true);
            //aiming with animator
        }
        if (Input.GetMouseButtonUp(1))
        {
            _isAiming = false;
            _crosshair.SetActive(false);
        }
    }

    void Fire()
    {
        if (!_isAiming) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_tipOfWeapon.position, _tipOfWeapon.forward, out RaycastHit hit))
            {
                Debug.Log("We Hit2");
            }
        }
    }
}
