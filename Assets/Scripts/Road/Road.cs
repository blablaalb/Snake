using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

public class Road : MonoBehaviour
{
    [SerializeField]
    private Transform _left;
    [SerializeField]
    private Transform _right;
    [SerializeField]
    private People _pplPrefab;
    private Color _mainColor;
    private Color _secondaryColor;
    private RoadSegment _roadSegment;

    internal void Awake()
    {
        _roadSegment = GetComponentInParent<RoadSegment>();
    }

    internal void OnEnable()
    {
        _mainColor = _roadSegment.MainColor;
        _secondaryColor = _roadSegment.SecondaryColor;
        SpawnPpl();
    }

    private void SpawnPpl()
    {
        int indx = Random.Range(0, 3);
        // 0 = left, 1 = right, 2 = both
        if (indx == 2)
        {
            Instantiate<People>(_pplPrefab, _left.position, Quaternion.identity, transform).SetColor(RandomColor());
            Instantiate<People>(_pplPrefab, _right.position, Quaternion.identity, transform).SetColor(RandomColor());
        }
        else
        if (indx == 0)
        {
            Instantiate<People>(_pplPrefab, _left.position, Quaternion.identity, transform).SetColor(RandomColor());
        }
        else if (indx == 1)
        {
            Instantiate<People>(_pplPrefab, _right.position, Quaternion.identity, transform).SetColor(RandomColor());
        }
    }

    private Color RandomColor()
    {
        int indx = Random.Range(0, 2);
        if (indx == 0) return _mainColor;
        return _secondaryColor;
    }
}