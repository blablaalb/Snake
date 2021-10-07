using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using Common;

public class SnakeMovement : Singleton<SnakeMovement>
{
    #region body
    [SerializeField]
    private List<GameObject> _bodyParts;
    // max distance between each part
    private float _maxZDistance = 1f;
    [SerializeField]
    private float _xDump;
    #endregion
    [SerializeField]
    private float _zSpeed = 1f;
    [SerializeField]
    private float _xSpeed = 10f; 

    public float ZSpeed => _zSpeed;

    internal void Update()
    {
        MoveZ();
        MoveX();
        FollowZ();
        FollowX();
    }

    private void MoveX()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var position = transform.position;
                position.x = Mathf.Lerp(position.x, hit.point.x, _xSpeed * Time.deltaTime);
                position.x = Mathf.Clamp(position.x, -Road.Width, Road.Width);
                transform.position = position;
            }
        }
    }

    private void MoveZ()
    {
        var position = transform.position;
        position.z += _zSpeed * Time.deltaTime;
        transform.position = position;
    }

    private void FollowZ()
    {
        int length = _bodyParts.Count;
        for (int i = 1; i < length; i++)
        {
            GameObject bp = _bodyParts[i];
            GameObject previousBp = _bodyParts[i - 1];
            float distance = Mathf.Abs(previousBp.transform.position.z - bp.transform.position.z);
            if (distance > _maxZDistance)
            {
                float z = previousBp.transform.position.z - 1f;
                Vector3 newPos = bp.transform.position;
                newPos.z = z;
                bp.transform.position = newPos;
            }
        }
    }

    private void FollowX()
    {
        int length = _bodyParts.Count;
        for (int i = 1; i < length; i++)
        {
            GameObject bp = _bodyParts[i];
            GameObject previousBp = _bodyParts[i - 1];
            float distance = Mathf.Abs(previousBp.transform.position.x - bp.transform.position.x);
            float x = previousBp.transform.position.x;
            x = Mathf.Lerp(bp.transform.position.x, x, _xDump);
            Vector3 newPos = bp.transform.position;
            newPos.x = x;
            bp.transform.position = newPos;
        }
    }
}