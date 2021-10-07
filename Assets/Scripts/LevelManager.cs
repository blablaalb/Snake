using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using Common;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private Color[] _colors;

    public Color MainColor { get; private set; }
    public Color SecondaryColor { get; private set; }

    override protected void Awake()
    {
        base.Awake();
        RandomizeColors();
    }

    public void RandomizeColors()
    {
        int mainIndx = Random.Range(0, _colors.Length);
        int secondaryIndx = Random.Range(0, _colors.Length);
        if (secondaryIndx == mainIndx) RandomizeColors();

        MainColor = _colors[mainIndx];
        SecondaryColor = _colors[secondaryIndx];
    }

}