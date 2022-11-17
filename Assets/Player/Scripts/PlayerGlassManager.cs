using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlassManager : MonoBehaviour
{
    [SerializeField] float _glassOnDuration = 5f;

    Player _player;

    GlassState _currentGlassState;
    public GlassState CurrentGlassState { get { return _currentGlassState; } }

    private void Awake()
    {
        _player = GetComponent<Player>();

        _currentGlassState = GlassState.GlassOn;
    }

    void Update()
    {
    }

    public void ChangeGlassState()
    {
        CancelInvoke(nameof(WearGlass));

        if (_currentGlassState == GlassState.GlassOn)
        {
            TakeOffGlass();
            Invoke(nameof(WearGlass), _glassOnDuration);
            return;
        }
        WearGlass();
    }

    void WearGlass()
    {
        _currentGlassState = GlassState.GlassOn;
        HumanPool.Instance.ChangeBodies(GlassState.GlassOn);
        Debug.Log(_currentGlassState);

        _player.Animator.SetTrigger("_changeGlasses");

        _player.PlayerInputManager.DisableAiming();
    }

    void TakeOffGlass()
    {
        _currentGlassState = GlassState.GlassOff;
        HumanPool.Instance.ChangeBodies(GlassState.GlassOff);
        Debug.Log(_currentGlassState);
        
        _player.Animator.SetTrigger("_changeGlasses");
    }

    public enum GlassState
    {
        GlassOn,
        GlassOff,
    }
}
