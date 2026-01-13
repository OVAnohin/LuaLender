using System;
using UnityEngine;

public class GameFlowController : MonoBehaviour
{
    public GameState CurrentState { get; private set; }

    public event Action<GameState> StateChanged;

    private void Start()
    {
        SetState(GameState.Ready);
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        StateChanged?.Invoke(CurrentState);
    }
}
