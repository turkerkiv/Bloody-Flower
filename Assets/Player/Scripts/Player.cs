using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerAttack PlayerAttack { get; private set; }
    public PlayerGlassManager PlayerGlassManager { get; private set; }
    public PlayerInputManager PlayerInputManager { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }

    public Animator Animator { get; private set; }

    private void Awake()
    {
        PlayerAttack = GetComponent<PlayerAttack>();
        PlayerGlassManager = GetComponentInChildren<PlayerGlassManager>();
        PlayerInputManager = GetComponent<PlayerInputManager>();
        PlayerMovement = GetComponent<PlayerMovement>();

        Animator = GetComponent<Animator>();
    }
}
