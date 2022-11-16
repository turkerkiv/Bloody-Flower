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

    void Awake()
    {
        _crosshair.SetActive(false);
        _inputManager = GetComponent<PlayerInputManager>();

        _animator = GetComponent<Animator>();
        _animAimingBoolHash = Animator.StringToHash("_isAiming");
    }

    void Update()
    {
        Aim();
    }

    public void Fire()
    {
        //fixed delaying but not it is kinda unrealistic
        _animator.Play("Shooting", 0, 0.25f);

        if (!Physics.Raycast(_tipOfWeapon.position, _tipOfWeapon.forward, out RaycastHit hit)) { return; }
        HumanAI human = hit.transform.GetComponentInParent<HumanAI>();
        if (human != null)
        {
            human.gameObject.SetActive(false);
        }
    }

    void Aim()
    {
        if (_crosshair.activeInHierarchy == _inputManager.IsAiming) { return; }

        _crosshair.SetActive(_inputManager.IsAiming);
        _animator.SetBool(_animAimingBoolHash, _inputManager.IsAiming);
    }
}
