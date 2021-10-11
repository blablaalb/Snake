using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

public class RoadSegment : MonoBehaviour
{
    [SerializeField]
    private GameObject _colorLine;
    private Material _colorLineMaterial;
    private ParticleSystem[] _particles;

    public Color MainColor;
    public Color SecondaryColor;
    public static float Width = 10f;
    public static float Length = 90f;
    public static RoadSegment ActiveRoadSegment;
    public static Action<RoadSegment> ActiveRoadSegmentChanged;


    internal void Awake()
    {
        _colorLineMaterial = _colorLine.GetComponent<MeshRenderer>().material;
        _colorLineMaterial = new Material(_colorLineMaterial);
        _colorLine.GetComponent<MeshRenderer>().material = _colorLineMaterial;
        _particles = GetComponentsInChildren<ParticleSystem>();
        RandomizeColors();
    }

    public void RandomizeColors()
    {
        if (LevelManager.Instance != null)
        {
            int mainIndx = Random.Range(0, LevelManager.Instance.AvailableColors.Length);
            int secondaryIndx = Random.Range(0, LevelManager.Instance.AvailableColors.Length);
            if (secondaryIndx == mainIndx)
            {
                RandomizeColors();
                return;
            }

            MainColor = LevelManager.Instance.AvailableColors[mainIndx];
            SecondaryColor = LevelManager.Instance.AvailableColors[secondaryIndx];
            _colorLineMaterial.color = MainColor;
            foreach (var particle in _particles)
            {
                var main = particle.main;
                var color = MainColor;
                color.a = 1f;
                main.startColor = color;
            }
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