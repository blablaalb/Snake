using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using DG.Tweening;
using Common;

public class Fiverr : Singleton<Fiverr>
{
    private SnakeMovement _snakeMovement;
    [SerializeField]
    private float _duration = 5f;

    public bool Activated { get; private set; }
    public event Action FiverStarted;
    public event Action FiverFinished;

    override protected void Awake()
    {
        base.Awake();
        _snakeMovement = FindObjectOfType<SnakeMovement>();
    }

    internal void Start()
    {
        GameManager.Instance.GemsAdded += OnGemsAdded;
    }

    override protected void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.GemsAdded -= OnGemsAdded;
        base.OnDestroy();
    }

    internal void Update()
    {
        if (Activated)
            _snakeMovement.MoveX(0);
    }

    private void OnGemsAdded(int obj)
    {
        if (!Activated)
            if (GameManager.Instance.Gems > 3)
            {
                Activated = true;
                _snakeMovement.Controllable = false;
                float countdown = _duration;
                DOTween.To(() => countdown, x => countdown = x, 0f, _duration).OnComplete(() => FinishFiver());
                FiverStarted?.Invoke();
            }
    }

    private void FinishFiver()
    {
        _snakeMovement.Controllable = true;
        GameManager.Instance.SubtractGems(3);
        Activated = false;
        FiverFinished?.Invoke();
    }

}