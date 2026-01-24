using UnityEngine;

[RequireComponent(typeof(AppFlowController))]
public class AppBootstrap : MonoBehaviour
{
    public static AppFlowController AppFlow { get; private set; }

    private void Awake()
    {
        if (AppFlow == null)
        {
            AppFlow = GetComponent<AppFlowController>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        AppFlow.SetState(AppState.MainMenu);
    }
}

