using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlassManager : MonoBehaviour
{
    [SerializeField] Animator _glassesAnimator;
    [SerializeField] Canvas _glassesStateCanvas;
    [SerializeField] float _glassOnDuration = 5f;

    Player _player;

    GlassState _currentGlassState;
    public GlassState CurrentGlassState { get { return _currentGlassState; } }

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        WearGlasses();
    }

    void Update()
    {
    }

    public void ChangeGlassState()
    {
        CancelInvoke(nameof(WearGlasses));

        if (_currentGlassState == GlassState.GlassOn)
        {
            TakeOffGlasses();
            Invoke(nameof(WearGlasses), _glassOnDuration);
            return;
        }

        WearGlasses();
    }

    void WearGlasses()
    {
        _currentGlassState = GlassState.GlassOn;
        HumanPool.Instance.ChangeBodies(GlassState.GlassOn);

        _player.PlayerInputManager.DisableAiming();

        _glassesAnimator.SetBool("_isGlassesWore", true);
    }

    void TakeOffGlasses()
    {
        _currentGlassState = GlassState.GlassOff;
        HumanPool.Instance.ChangeBodies(GlassState.GlassOff);

        _glassesAnimator.SetBool("_isGlassesWore", false);
    }

    public enum GlassState
    {
        GlassOn,
        GlassOff,
    }
}
