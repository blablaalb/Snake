using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using TMPro;

public class FiverUI : MonoBehaviour
{
    private TextMeshProUGUI _tmpro;

    internal void Awake()
    {
        _tmpro = GetComponentInChildren<TextMeshProUGUI>();
    }

    internal void Start()
    {
        Fiverr.Instance.FiverStarted += () => gameObject.SetActive(true);
        Fiverr.Instance.FiverFinished += () => gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    internal void LateUpdate()
    {
        _tmpro.text = $"Fiver: { Fiverr.Instance.Countdown:#,#0}";
    }
}