using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

public class People : MonoBehaviour
{
    private Color _color;
    private Material[] _materials;

    public Color Color => _color;

    internal void Awake()
    {
        _materials = GetComponentsInChildren<MeshRenderer>().SelectMany(x => x.materials).ToArray();
    }

    public void SetColor(Color color)
    {
        _color = color;
        foreach (var mat in _materials)
        {
            mat.color = color;
        }
    }
}