using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _lostPanel;
    [SerializeField]
    private GameObject _winPanel;

    internal void Start()
    {
        LevelManager.Instance.SessionFinished += OnSessionFinished;
    }

    private void OnSessionFinished(SessionResult result)
    {
        if (result == SessionResult.Lost)
        {
            _lostPanel.SetActive(true);
        }

        else if (result == SessionResult.Won)
        {
            _winPanel.SetActive(true);
        }
    }
}