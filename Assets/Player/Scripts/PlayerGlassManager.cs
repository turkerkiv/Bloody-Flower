using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlassManager : MonoBehaviour
{
    [SerializeField] float _glassOffDuration = 5f;

    Animator _glassesAnimator;
    Canvas _glassesStateCanvas;
    Player _player;
    GlassState _currentGlassState;

    public GlassState CurrentGlassState { get { return _currentGlassState; } }

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _glassesAnimator = GetComponent<Animator>();
        _glassesStateCanvas = GetComponentInChildren<Canvas>();
    }

    private void Start()
    {
        _glassesAnimator.SetBool("_isGlassesWore", true);
    }

    public void ChangeGlassState()
    {
        if (_currentGlassState == GlassState.GlassOn)
        {
            _glassesAnimator.SetBool("_isGlassesWore", false);
            return;
        }

        _glassesAnimator.SetBool("_isGlassesWore", true);
    }

    void WearGlasses()
    {
        CancelInvoke(nameof(ChangeGlassState));

        _currentGlassState = GlassState.GlassOn;
        HumanPool.Instance.ChangeBodies(GlassState.GlassOn);

        _player.PlayerInputManager.DisableAiming();

        _glassesStateCanvas.enabled = true;
    }

    void TakeOffGlasses()
    {
        Invoke(nameof(ChangeGlassState), _glassOffDuration);

        _currentGlassState = GlassState.GlassOff;
        HumanPool.Instance.ChangeBodies(GlassState.GlassOff);

        _glassesStateCanvas.enabled = false;
    }

    public enum GlassState
    {
        GlassOn,
        GlassOff,
    }
}
