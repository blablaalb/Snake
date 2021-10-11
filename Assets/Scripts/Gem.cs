using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using DG.Tweening;

public class Gem : MonoBehaviour
{
    private bool _added;

    internal void OnTriggerEnter(Collider other)
    {
        if (_added) return;
        if (other.GetComponent<Snake>() is Snake snake)
        {
            GameManager.Instance.AddGems(1);
            transform.SetParent(snake.transform);
            transform.DOLocalMove(Vector3.zero, duration: .5f);
            transform.DOScale(Vector3.zero, 0.5f);
            _added = true;
        }
    }
}