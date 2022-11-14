using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject _crosshair;
    [SerializeField] Transform _tipOfWeapon;

    Animator _animator;

    bool _isAiming;

    void Awake()
    {
        _crosshair.SetActive(false);
        _animator = GetComponent<Animator>();
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
            _animator.SetBool(nameof(_isAiming), _isAiming);
        }
        if (Input.GetMouseButtonUp(1))
        {
            _isAiming = false;
            _crosshair.SetActive(false);
            _animator.SetBool(nameof(_isAiming), _isAiming);
        }
    }

    void Fire()
    {
        if (!_isAiming || !Input.GetMouseButtonDown(0) || !Physics.Raycast(_tipOfWeapon.position, _tipOfWeapon.forward, out RaycastHit hit)) { return; }

        HumanAI human = hit.transform.GetComponentInParent<HumanAI>();

        if (human != null)
        {
            human.gameObject.SetActive(false);
        }
    }
}
