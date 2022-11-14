using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPool : MonoBehaviour
{
    [SerializeField] List<GameObject> _humanBodies;
    [SerializeField] HumanAI _humanPrefab;
    [SerializeField] int _steadyHumanCount;
    [SerializeField] int _moverHumanCount;
    [SerializeField] int _killerHumanCount;
    [SerializeField] Vector3 _spawnArea;

    List<HumanAI> _humans = new List<HumanAI>();
    List<HumanAI> _steadyHumans = new List<HumanAI>();

    public List<HumanAI> SteadyHumans { get { return _steadyHumans; } }
    public static HumanPool Instance { get; private set; }

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

    void Start()
    {
        PopulatePool();
    }

    void Update()
    {

    }

    public void ChangeBodies(Player.GlassState state)
    {
        foreach (HumanAI human in _humans)
        {
            if (state == Player.GlassState.GlassOn)
            {
                human.ChangeBody(state);
            }
            else
            {
                human.ChangeBody(state);
            }
        }
    }

    void PopulatePool()
    {
        for (int i = 0; i < _steadyHumanCount; i++)
        {
            InstantiateHuman(HumanType.Steady);
        }

        for (int i = 0; i < _killerHumanCount; i++)
        {
            InstantiateHuman(HumanType.Killer);
        }

        for (int i = 0; i < _moverHumanCount; i++)
        {
            InstantiateHuman(HumanType.Mover);
        }
    }

    void InstantiateHuman(HumanType type)
    {
        HumanAI instance = Instantiate(_humanPrefab, GetRandomPosition(), Quaternion.identity, transform);
        instance.Type = type;
        _humans.Add(instance);

        int randomIndex = Random.Range(0, _humanBodies.Count);
        instance.SetGlassOffBody(_humanBodies[randomIndex]);

        if (type == HumanType.Steady)
        {
            _steadyHumans.Add(instance);
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-_spawnArea.x, _spawnArea.x);
        float z = Random.Range(-_spawnArea.z, _spawnArea.z);

        return new Vector3(x, 0, z);
    }

    public enum HumanType
    {
        Steady,
        Killer,
        Mover,
    }
}