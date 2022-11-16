using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject _crosshair;
    [SerializeField] Transform _tipOfWeapon;

    Animator _animator;
    PlayerInputManager _inputManager;

    int _animAimingBoolHash;
    int _animFiringTriggerHash;

    void Awake()
    {
        _crosshair.SetActive(false);
        _inputManager = GetComponent<PlayerInputManager>();

        _animator = GetComponent<Animator>();
        _animAimingBoolHash = Animator.StringToHash("_isAiming");
        _animFiringTriggerHash = Animator.StringToHash("_fire");
    }

    void Update()
    {
        Aim();
    }

    void Aim()
    {
        if (_inputManager.IsAiming)
        {
            _crosshair.SetActive(true);
        }
        else
        {
            _crosshair.SetActive(false);
        }

        if (_animator.GetBool(_animAimingBoolHash) == _inputManager.IsAiming) { return; }
        _animator.SetBool(_animAimingBoolHash, _inputManager.IsAiming);
    }

    public void Fire()
    {
        if (!_inputManager.IsAiming) { return; }
        _animator.SetTrigger(_animFiringTriggerHash);

        if (!Physics.Raycast(_tipOfWeapon.position, _tipOfWeapon.forward, out RaycastHit hit)) { return; }
        HumanAI human = hit.transform.GetComponentInParent<HumanAI>();
        if (human != null)
        {
            human.gameObject.SetActive(false);
        }
    }
}
