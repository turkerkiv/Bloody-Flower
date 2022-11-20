using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanAI : MonoBehaviour
{
    [SerializeField] GameObject _glassOnBody;
    [SerializeField] GameObject _glassOffBody;
    [SerializeField][Range(0, 100)] int _actionPercent = 10;

    NavMeshAgent _navMeshAgent;
    HumanAI _targetHuman;
    Animator[] _animators;

    public HumanPool.HumanType Type { get; set; }

    bool _tookAction;
    bool _isDead;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animators = GetComponentsInChildren<Animator>();
    }

    private void OnEnable()
    {
        _isDead = false;
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
        if (_isDead) { return; }

        if (Type != HumanPool.HumanType.Steady)
        {
            CheckForAction();
            LookToTarget();
        }
    }

    private void OnDisable()
    {
        _isDead = true;

        GameManager.Instance.ModifyTrustRate(Type);
    }

    public void SetGlassOffBody(GameObject body)
    {
        Instantiate(body, transform.position, Quaternion.identity, _glassOffBody.transform);
    }

    public void ChangeBody(PlayerGlassManager.GlassState state)
    {
        _glassOffBody.SetActive(state == PlayerGlassManager.GlassState.GlassOff);
        _glassOnBody.SetActive(state == PlayerGlassManager.GlassState.GlassOn);
    }

    void SelectRandomHuman()
    {
        _tookAction = false;

        int randomIndex = Random.Range(0, HumanPool.Instance.SteadyHumans.Count);

        _targetHuman = HumanPool.Instance.SteadyHumans[randomIndex];

        MoveToTarget();
    }

    void MoveToTarget()
    {
        if (_isDead) { return; }

        _navMeshAgent.SetDestination(_targetHuman.transform.position);
        SetAnimatorSpeedValue(1);
    }

    void CheckForAction()
    {
        //if close enough
        if (!_tookAction && Vector3.Distance(transform.position, _targetHuman.transform.position) <= _navMeshAgent.stoppingDistance)
        {
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

            //to make it once
            _tookAction = true;

            SetAnimatorSpeedValue(0);
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

    void SetAnimatorSpeedValue(float value)
    {
        foreach (Animator animator in _animators)
        {
            animator.SetFloat("_speed", value);
        }
    }
}
