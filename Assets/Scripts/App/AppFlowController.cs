using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppFlowController : MonoBehaviour
{
    public static AppFlowController Instance => AppBootstrap.Instance.AppFlow;

    public AppState CurrentState { get; private set; }
    public event Action<AppState> StateChanged;

    private void Awake()
    {
        // При старте через BootScene можно сразу поставить Boot
        if (CurrentState == default)
            SetState(AppState.Boot);
    }

    public void SetState(AppState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        StateChanged?.Invoke(CurrentState);
        LoadSceneForState(CurrentState);
    }

    private SceneNames GetSceneForState(AppState state)
    {
        switch (state)
        {
            case AppState.Boot:
                return SceneNames.BootScene;
            case AppState.MainMenu:
                return SceneNames.MainMenuScene;
            case AppState.Gameplay:
                return SceneNames.GameScene;
            case AppState.GameOver:
                return SceneNames.GameOverScene;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, "Unknown AppState");
        }
    }

    private void LoadSceneForState(AppState state)
    {
        SceneNames scene = GetSceneForState(state);
        SceneManager.LoadScene(scene.ToString());
    }

    public void Next()
    {
        switch (CurrentState)
        {
            case AppState.Boot:
                SetState(AppState.MainMenu);
                break;
            case AppState.MainMenu:
                SetState(AppState.Gameplay);
                break;
            case AppState.Gameplay:
                SetState(AppState.GameOver);
                break;
            case AppState.GameOver:
                SetState(AppState.MainMenu);
                break;
        }
    }

    public void RestartGameplay()
    {
        SetState(AppState.Gameplay);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
