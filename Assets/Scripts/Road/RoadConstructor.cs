using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using Common;

public class RoadConstructor : Singleton<RoadConstructor>
{
    [SerializeField]
    private GameObject _roadChunkPrefab;
    private List<GameObject> _roadChunks;

    public List<GameObject> RoadChunks => _roadChunks;

    override protected void Awake()
    {
        base.Awake();
        _roadChunks = new List<GameObject>();
    }

    internal void Start()
    {
        AddRoad(25);
    }

    public GameObject[] AddRoad(int count)
    {
        GameObject[] arr = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            arr[i] = AddRoad();
        }

        return arr;
    }

    public GameObject AddRoad()
    {
        Vector3 position = Vector3.zero;
        if (_roadChunks.Count > 0)
            position = _roadChunks.Last().transform.position;
        position.z += RoadSegment.Length;
        GameObject roadGo = Instantiate<GameObject>(_roadChunkPrefab, position, Quaternion.identity);
        _roadChunks.Add(roadGo);
        return roadGo;
    }
}