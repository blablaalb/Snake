using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

public class RoadSegment : MonoBehaviour
{
    public Color MainColor;
    public Color SecondaryColor;
    public static float Width = 10f;
    public static float Length = 90f;
    public static RoadSegment ActiveRoadSegment;
    public static Action<RoadSegment> ActiveRoadSegmentChanged;

    internal void Awake()
    {
        RandomizeColors();
    }

    public void RandomizeColors()
    {
        if (LevelManager.Instance != null){

        int mainIndx = Random.Range(0, LevelManager.Instance.AvailableColors.Length);
        int secondaryIndx = Random.Range(0, LevelManager.Instance.AvailableColors.Length);
        if (secondaryIndx == mainIndx)
        {
            RandomizeColors();
            return;
        }

        MainColor = LevelManager.Instance.AvailableColors[mainIndx];
        SecondaryColor = LevelManager.Instance.AvailableColors[secondaryIndx];
        }
    }

    internal void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Snake>())
        {
            if (this != ActiveRoadSegment)
            {
                ActiveRoadSegment = this;
                ActiveRoadSegmentChanged?.Invoke(this);
            }
        }
    }


}