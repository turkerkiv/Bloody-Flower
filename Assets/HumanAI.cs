using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanAI : MonoBehaviour
{
    [SerializeField][Range(0, 100)] int _actionPercent = 10;
    public HumanPool HumanPool { get; set; }

    NavMeshAgent _navMeshAgent;
    HumanAI _targetHuman;

    public HumanPool.HumanType Type { get; set; }

    bool _didAction;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (Type != HumanPool.HumanType.Steady)
        {
            SelectRandomHuman();
        }
    }

    private void Update()
    {
        if (Type != HumanPool.HumanType.Steady)
        {
            CheckForAction();
            LookToTarget();
        }
    }

    private void OnDisable()
    {
    }

    void SelectRandomHuman()
    {
        _didAction = false;

        int randomIndex = Random.Range(0, HumanPool.SteadyHumans.Count);

        _targetHuman = HumanPool.SteadyHumans[randomIndex];

        MoveToTarget();
    }

    void MoveToTarget()
    {
        _navMeshAgent.SetDestination(_targetHuman.transform.position);
    }

    void CheckForAction()
    {
        //if close enough
        if (!_didAction && Vector3.Distance(transform.position, _targetHuman.transform.position) <= _navMeshAgent.stoppingDistance)
        {
            //to make it once
            _didAction = true;

            //if it is killer, roll dice to kill or not 
            if (Type == HumanPool.HumanType.Killer && CanStartAction())
            {
                //can put invoke to wait some random time to kill before going to another one
                Debug.Log("Killed our target");
                Invoke(nameof(SelectRandomHuman), 5f);
            }
            else
            {
                //if it is not killer or killer but can not kill then select another one
                Debug.Log("Tried");
                Invoke(nameof(SelectRandomHuman), 5f);
            }
        }
    }

    bool CanStartAction()
    {
        List<int> states = new List<int>();

        for (int i = 0; i < _actionPercent; i++)
        {
            states.Add(1);
        }
        for (int i = 0; i < 100 - _actionPercent; i++)
        {
            states.Add(0);
        }

        int randomIndex = Random.Range(0, states.Count);

        if (states[randomIndex] == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void LookToTarget()
    {
        transform.LookAt(_targetHuman.transform);
    }
}
