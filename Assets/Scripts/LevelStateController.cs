using System;
using UnityEngine;

public class LevelStateController : MonoBehaviour
{
    public LevelPhase CurrentPhase { get; private set; }

    public event Action<LevelPhase> PhaseChanged;

    private void Start()
    {
        SetPhase(LevelPhase.Ready);
    }

    public void SetPhase(LevelPhase newState)
    {
        if (CurrentPhase == newState)
            return;

        CurrentPhase = newState;
        PhaseChanged?.Invoke(CurrentPhase);
    }
}
