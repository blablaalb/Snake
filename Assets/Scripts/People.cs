using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using System.Collections;
using DG.Tweening;

public class People : MonoBehaviour, IEatable
{
    private Color _color;
    private Material[] _materials;
    private GameObject[] _people;

    public Color Color => _color;

    internal void Awake()
    {
        _materials = GetComponentsInChildren<MeshRenderer>().SelectMany(x => x.materials).ToArray();
        _people = transform.GetComponentsInChildren<Transform>().Where(x => x.gameObject != this.gameObject).Select(x => x.gameObject).ToArray();
    }

    public void SetColor(Color color)
    {
        _color = color;
        foreach (var mat in _materials)
        {
            mat.color = color;
        }
    }

    public void Eaten()
    {
        var snake = FindObjectOfType<Snake>();
        var scale = new Vector3(0.1f, 0.1f, 0.1f);
        foreach (var person in _people)
        {
            person.transform.SetParent(snake.transform);
            person.transform.DOScale(scale, duration: 0.8f);
            person.transform.DOLocalMove(Vector3.zero, duration: 1f).OnComplete(() => person.gameObject.SetActive(false));
        }
    }
}