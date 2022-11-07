using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPool : MonoBehaviour
{
    [SerializeField] HumanAI _humanPrefab;
    [SerializeField] int _steadyHumanCount;
    [SerializeField] int _moverHumanCount;
    [SerializeField] int _killerHumanCount;
    [SerializeField] Vector3 _spawnArea;

    List<HumanAI> _steadyHumans = new List<HumanAI>();
    public List<HumanAI> SteadyHumans { get { return _steadyHumans; } }

    void Start()
    {
        PopulatePool();
    }

    void Update()
    {

    }

    void PopulatePool()
    {
        for (int i = 0; i < _steadyHumanCount; i++)
        {
            HumanAI instance = Instantiate(_humanPrefab, GetRandomPosition(), Quaternion.identity);
            instance.transform.SetParent(transform);
            instance.HumanPool = this;
            instance.Type = HumanType.Steady;
            instance.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;

            _steadyHumans.Add(instance);
        }

        for (int i = 0; i < _killerHumanCount; i++)
        {
            HumanAI instance = Instantiate(_humanPrefab, GetRandomPosition(), Quaternion.identity);
            instance.transform.SetParent(transform);
            instance.HumanPool = this;
            instance.Type = HumanType.Killer;
        }

        for (int i = 0; i < _moverHumanCount; i++)
        {
            HumanAI instance = Instantiate(_humanPrefab, GetRandomPosition(), Quaternion.identity);
            instance.transform.SetParent(transform);
            instance.HumanPool = this;
            instance.Type = HumanType.Mover;
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