using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

public class OnTriggerCallback : MonoBehaviour
{
    public Action<Collider> TriggerEnter;
    public Action<Collider> TriggerExit;

    internal void OnTriggerEnter(Collider other)
    {
        TriggerEnter?.Invoke(other);
    }

    internal void OnTriggerExit(Collider other)
    {
        TriggerExit?.Invoke(other);
    }
}