using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlassManager : MonoBehaviour
{
    [SerializeField] GameObject _glasses;
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
        WearGlass();
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

        _player.PlayerInputManager.DisableAiming();

        //will change here
        _player.Animator.SetTrigger("_changeGlasses");
        _glasses.SetActive(true);
        Invoke(nameof(EnableGlassesCanvas), 1f);
    }

    void TakeOffGlass()
    {
        _currentGlassState = GlassState.GlassOff;
        HumanPool.Instance.ChangeBodies(GlassState.GlassOff);

        //will change here
        _player.Animator.SetTrigger("_changeGlasses");
        _glassesStateCanvas.enabled = false;
        Invoke(nameof(DisableGlassesObject), 1f);
    }

    void EnableGlassesCanvas()
    {
        _glassesStateCanvas.enabled = true;
    }

    void DisableGlassesObject()
    {
        _glasses.SetActive(false);
    }

    public enum GlassState
    {
        GlassOn,
        GlassOff,
    }
}
