using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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

        if (_glassState == GlassState.GlassOn)
        {
            _glassState = GlassState.GlassOff;
            HumanPool.Instance.ChangeBodies(GlassState.GlassOff);
            return;
        }

        _glassState = GlassState.GlassOn;
        HumanPool.Instance.ChangeBodies(GlassState.GlassOn);
    }

    public enum GlassState
    {
        GlassOn,
        GlassOff,
    }
}
