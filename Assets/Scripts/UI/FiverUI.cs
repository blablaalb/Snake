using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

public class FiverUI : MonoBehaviour
{
    internal void Start()
    {
        Fiverr.Instance.FiverStarted += () => gameObject.SetActive(true);
        Fiverr.Instance.FiverFinished += () => gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}