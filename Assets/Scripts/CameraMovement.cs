using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

public class CameraMovement : MonoBehaviour
{
    internal void Update()
    {
        Vector3 position = transform.position;
        position.z += SnakeMovement.Instance.ZSpeed * Time.deltaTime;
        transform.position = position;
    }
}