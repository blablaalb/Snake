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
    [Header("Body Parts")]
    [SerializeField]
    private GameObject _bodyPartPrefab;
    [SerializeField]
    private List<GameObject> _bodyParts;
    // max distance between each part
    [SerializeField]
    private float _maxZDistance = 0.5f;
    [SerializeField]
    private float _xDump;
    [SerializeField]
    private float _maxXDistance = 0.5f;
    #endregion body
    [Header("Movement")]
    [SerializeField]
    private float _zSpeed = 1f;
    [SerializeField]
    private float _xSpeed = 10f;
    private List<People> _collided;

    public float ZSpeed => _zSpeed;

    override protected void Awake()
    {
        base.Awake();
        _collided = new List<People>();
    }

    internal void Start()
    {
        UpdateColor();
    }

    internal void Update()
    {
        MoveZ();
        MoveX();
        FollowZ();
        FollowX();
    }

    public void AddBodyParts(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddBodyPart();
        }
    }

    public void AddBodyPart()
    {
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        if (_bodyParts.Count > 0)
        {
            GameObject last = _bodyParts.Last();
            position = last.transform.position + -last.transform.forward * _maxZDistance;
            rotation = last.transform.rotation;
        }
        GameObject bp = Instantiate<GameObject>(_bodyPartPrefab, position, rotation);
        _bodyParts.Add(bp);
        UpdateColor();
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
                position.x = Mathf.Clamp(position.x, -RoadSegment.Width, RoadSegment.Width);
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
                float z = previousBp.transform.position.z - _maxZDistance;
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
            float dirNorm = (previousBp.transform.position.x - bp.transform.position.x) / Mathf.Abs(previousBp.transform.position.x - bp.transform.position.x);
            var dir = previousBp.transform.position - bp.transform.position;
            float x = previousBp.transform.position.x;
            x = Mathf.Lerp(bp.transform.position.x, x, _xDump);
            if (distance > _maxXDistance)
            {
                x += (distance - _maxXDistance) * dirNorm;
            }
            Vector3 newPos = bp.transform.position;
            newPos.x = x;
            bp.transform.position = newPos;
            bp.transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    internal void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<People>() is People ppl)
        {
            if (!_collided.Contains(ppl))
            {
                _collided.Add(ppl);
                if (ppl.Color == LevelManager.Instance.MainColor)
                {
                    AddBodyPart();
                }
                else if (ppl.Color == LevelManager.Instance.SecondaryColor)
                {
                    LevelManager.Instance.Lost();
                }
            }
        }
    }

    public void UpdateColor()
    {
        for (int i = 0; i < _bodyParts.Count; i++)
        {
            _bodyParts[i].GetComponentInChildren<MeshRenderer>().material.color = LevelManager.Instance.MainColor;
        }
    }
}