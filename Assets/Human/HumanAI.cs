using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanAI : MonoBehaviour
{
    [SerializeField] Transform _glassOffBodyParent;
    [SerializeField] GameObject _glassOnBodyGraphics;
    [SerializeField] GameObject _glassOffBodyGraphics;
    [SerializeField][Range(0, 10)] int _actionProbability = 1;

    NavMeshAgent _navMeshAgent;
    HumanAI _targetHuman;
    Animator[] _animators;

    public HumanPool.HumanType Type { get; set; }

    bool _tookAction;
    bool _isDead;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
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

    public void InstantiateGlassOffBody(GameObject body)
    {
        GameObject instance = Instantiate(body, transform.position, Quaternion.identity, _glassOffBodyParent);
        _glassOffBodyGraphics = instance.transform.Find("BodyGraphics").gameObject;
        _glassOffBodyGraphics.SetActive(false);

        _animators = GetComponentsInChildren<Animator>();
    }

    public void ChangeBody(PlayerGlassManager.GlassState state)
    {
        bool isGlassWore = state == PlayerGlassManager.GlassState.GlassOn;

        _glassOffBodyGraphics.SetActive(!isGlassWore);
        _glassOnBodyGraphics.SetActive(isGlassWore);
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

        for (int i = 0; i < _actionProbability; i++)
        {
            states.Add(1);
        }
        for (int i = 0; i < 10 - _actionProbability; i++)
        {
            states.Add(0);
        }

        int randomIndex = Random.Range(0, states.Count);

        return states[randomIndex] == 1;
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

        Debug.Log(_animators.Length);
    }
}
