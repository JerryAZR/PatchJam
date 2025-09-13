using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public enum LevelState
{
    Locked,
    Ready,
    Cleared
}

public class ProgressManager : MMSingleton<ProgressManager>
{
    Dictionary<string, LevelState> _levelStates = new();
    [SerializeField] private string[] _allLevels;

    public LevelState GetState(string levelName) =>
        _levelStates.GetValueOrDefault(levelName, LevelState.Locked);

    public void SetLevelCleared(string levelName)
    {
        // Set this level as cleared and unlock next level
        _levelStates[levelName] = LevelState.Cleared;
    }

    private void Start()
    {
        if (_allLevels.Length > 0 && GetState(_allLevels[0]) == LevelState.Locked)
        {
            _levelStates[_allLevels[0]] = LevelState.Ready;
        }
    }
}
