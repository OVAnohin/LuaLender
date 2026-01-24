using System;
using UnityEngine;

public class LevelStateController : MonoBehaviour
{
    public LevelPhase CurrentState { get; private set; }

    public event Action<LevelPhase> StateChanged;

    private void Start()
    {
        SetState(LevelPhase.Ready);
    }

    public void SetState(LevelPhase newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        StateChanged?.Invoke(CurrentState);
    }
}
