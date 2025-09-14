using System;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;

public enum LevelState
{
    Locked,
    Ready,
    Cleared
}

public class ProgressManager : MMPersistentSingleton<ProgressManager>
{
    Dictionary<string, LevelState> _levelStates = new();
    Dictionary<string, int> _levelOrder;
    [SerializeField] private GameObject _gatesRoot;
    private LevelEntrance[] _entranceArray;

    public LevelState GetState(string levelName) =>
        _levelStates.GetValueOrDefault(levelName, LevelState.Locked);

    public void SetLevelCleared(string levelName)
    {
        // Set this level as cleared and unlock next level
        _levelStates[levelName] = LevelState.Cleared;
        int next = _levelOrder[levelName] + 1;
        if (GetState(GetLevelName(next)) == LevelState.Locked)
        {
            _levelStates[GetLevelName(next)] = LevelState.Ready;
        }
    }

    private void Start()
    {
        if (_entranceArray != null || _gatesRoot == null) return;
        _entranceArray = _gatesRoot.GetComponentsInChildren<LevelEntrance>();
        _levelOrder = Enumerable.Range(0, _entranceArray.Length)
            .ToDictionary(i => GetLevelName(i), i => i);
        if (GetState("Level1") == LevelState.Locked)
        {
            _levelStates["Level1"] = LevelState.Ready;
        }
    }

    private string GetLevelName(int index) => $"Level{index + 1}";
}
