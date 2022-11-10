using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _glassOnDuration = 5f;

    GlassState _glassState;

    void Start()
    {

    }

    void Update()
    {
        SetGlassState();
    }

    void SetGlassState()
    {
        if (!Input.GetKeyDown(KeyCode.F)) { return; }

        CancelInvoke(nameof(WearGlass));

        if (_glassState == GlassState.GlassOn)
        {
            TakeOffGlass();
            Invoke(nameof(WearGlass), _glassOnDuration);
            return;
        }

        WearGlass();
        Debug.Log(Time.time);
    }

    void WearGlass()
    {
        _glassState = GlassState.GlassOn;
        HumanPool.Instance.ChangeBodies(GlassState.GlassOn);
    }

    void TakeOffGlass()
    {
        _glassState = GlassState.GlassOff;
        HumanPool.Instance.ChangeBodies(GlassState.GlassOff);
        Debug.Log(Time.time);
    }

    public enum GlassState
    {
        GlassOn,
        GlassOff,
    }
}
