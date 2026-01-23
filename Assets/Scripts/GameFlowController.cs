using System;
using UnityEngine;

public class GameFlowController : MonoBehaviour
{
    public GamePhase CurrentState { get; private set; }

    public event Action<GamePhase> StateChanged;

    private void Start()
    {
        SetState(GamePhase.Ready);
    }

    public void SetState(GamePhase newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        StateChanged?.Invoke(CurrentState);
    }
}
