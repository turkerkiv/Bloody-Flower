using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField][Range(0, 100)] int _trustRatePenaltyAmount = 25;
    [SerializeField][Range(0, 100)] int _trustRateRewardAmount = 10;
    int _trustRate;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }


    public void ModifyTrustRate(HumanPool.HumanType type)
    {
        if (type != HumanPool.HumanType.Killer)
        {
            _trustRate -= _trustRatePenaltyAmount;
        }
        else
        {
            _trustRate += _trustRateRewardAmount;
        }

        Debug.Log(_trustRate);
    }
}
