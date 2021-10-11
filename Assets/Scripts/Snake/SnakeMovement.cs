using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using Common;
using DG.Tweening;
using System.Collections.ObjectModel;

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
    private bool _move;
    [SerializeField]
    private Transform _largeCollider;
    private List<IEatable> _eaten;

    public float ZSpeed => _zSpeed;
    public bool Controllable { get; set; }
    public ReadOnlyCollection<IEatable> Eaten => new ReadOnlyCollection<IEatable>(_eaten);

    override protected void Awake()
    {
        base.Awake();
        _eaten = new List<IEatable>();
        _collided = new List<People>();
        _move = true;
        Controllable = true;
    }

    internal void Start()
    {
        UpdateColor();
        LevelManager.Instance.SessionFinished += (_) => Stop();
        var onTriggerCallback = _largeCollider.GetComponent<OnTriggerCallback>();
        Fiverr.Instance.FiverStarted += () =>
        {
            onTriggerCallback.TriggerEnter = (collider) =>
            {
                if (Fiverr.Instance.Activated)
                    if (collider.GetComponent<IEatable>() is IEatable eatable)
                    {
                        Eat(eatable);
                    }
            };
        };
    }

    internal void Update()
    {
        if (!_move) return;
        MoveZ();
        if (Controllable)
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
                MoveX(hit.point.x);
            }
        }
    }

    public void MoveX(float x)
    {
        var position = transform.position;
        position.x = Mathf.Lerp(position.x, x, _xSpeed * Time.deltaTime);
        position.x = Mathf.Clamp(position.x, -(RoadSegment.Width / 2 -1), RoadSegment.Width / 2- 1);
        transform.position = position;
    }

    private void MoveZ()
    {
        var position = transform.position;
        position.z += _zSpeed * Time.deltaTime;
        transform.position = position;
        Vector3 largeColliderPosition = transform.position + new Vector3(0, 0, 1f);
        largeColliderPosition.x = 0f;
        _largeCollider.transform.position = largeColliderPosition;
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
            x = Mathf.Lerp(bp.transform.position.x, x, _xDump * Time.deltaTime);
            if (distance > _maxXDistance)
            {
                // x += (distance - _maxXDistance) * dirNorm;
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
                    Eat(ppl);
                }
                else if (ppl.Color == LevelManager.Instance.SecondaryColor)
                {
                    if (!_eaten.Contains(ppl))
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

    public void Stop()
    {
        _move = false;
    }

    public void Eat(IEatable eatable)
    {
        _eaten.Add(eatable);
        eatable.Eaten();
    }
}