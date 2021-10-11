using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using TMPro;

public class GemsUI : MonoBehaviour
{
    private TextMeshProUGUI _tmpro;

    internal void Awake()
    {
        _tmpro = GetComponent<TextMeshProUGUI>();
    }

    internal void Start()
    {
        GameManager.Instance.GemsAdded += OnGemsAdded;
        GameManager.Instance.GemSubtracted += OnGemsSubtracted;
    }

    internal void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GemsAdded -= OnGemsAdded;
            GameManager.Instance.GemSubtracted -= OnGemsSubtracted;
        }
    }

    private void OnGemsAdded(int count)
    {
        _tmpro.SetText(GameManager.Instance.Gems.ToString());
    }

    private void OnGemsSubtracted(int count)
    {
        _tmpro.SetText(GameManager.Instance.Gems.ToString());
    }
}