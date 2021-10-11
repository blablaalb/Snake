using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using DG.Tweening;

public class Obstacle : MonoBehaviour
{
    private Transform _snake;

    internal void Awake()
    {
        _snake = FindObjectOfType<Snake>().transform;
    }

    internal void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Snake>())
        {
            if (!Fiverr.Instance.Activated)
                LevelManager.Instance.Lost();
            else Eaten();
        }
    }

    public void Eaten()
    {
        transform.SetParent(_snake);
        transform.DOScale(Vector3.zero, duration: 0.25f);
        transform.DOLocalMove(Vector3.zero, duration:0.5f).OnComplete(() => gameObject.SetActive(false));
    }
}