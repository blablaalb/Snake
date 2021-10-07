using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

public class RoadConstructor : MonoBehaviour
{
    [SerializeField]
    private GameObject _roadChunkPrefab;
    private List<GameObject> _roadChunks;

    internal void Awake()
    {
        _roadChunks = new List<GameObject>();
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
        position.z += Road.Length;
        GameObject roadGo = Instantiate<GameObject>(_roadChunkPrefab, position, Quaternion.identity);
        _roadChunks.Add(roadGo);
        return roadGo;
    }
}