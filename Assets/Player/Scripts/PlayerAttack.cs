using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject _crosshair;
    [SerializeField] Transform _tipOfWeapon;

    Player _player;

    int _animAimingBoolHash;

    void Awake()
    {
        _crosshair.SetActive(false);
        _player = GetComponent<Player>();

        _animAimingBoolHash = Animator.StringToHash("_isAiming");
    }

    void Update()
    {
        Aim();
    }

    public void Fire()
    {
        //fixed delaying but not it is kinda unrealistic
        _player.Animator.Play("Shooting", 0, 0.25f);

        if (!Physics.Raycast(_tipOfWeapon.position, _tipOfWeapon.forward, out RaycastHit hit)) { return; }
        HumanAI human = hit.transform.GetComponentInParent<HumanAI>();
        if (human != null)
        {
            human.gameObject.SetActive(false);
        }
    }

    void Aim()
    {
        if (_crosshair.activeInHierarchy == _player.PlayerInputManager.IsAiming) { return; }

        _crosshair.SetActive(_player.PlayerInputManager.IsAiming);
        _player.Animator.SetBool(_animAimingBoolHash, _player.PlayerInputManager.IsAiming);
    }
}
