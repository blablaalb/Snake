using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using Common;

public class GameManager : Singleton<GameManager>
{
    public int Gems { get; private set; }
    public event Action<int> GemsAdded;
    public event Action<int> GemSubtracted;

    public void AddGems(int count)
    {
        Gems += count;
        GemsAdded(count);
    }

    public void SubtractGems(int count)
    {
        int subtracted = count;
        if (Gems < count)
            subtracted = Gems;
        Gems -= subtracted;
        GemSubtracted?.Invoke(subtracted);
    }
}