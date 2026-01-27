using UnityEngine;

[RequireComponent(typeof(AppFlowController))]
public class AppBootstrap : MonoBehaviour
{
    public static AppBootstrap Instance { get; private set; }

    public AppFlowController AppFlow { get; private set; }
    public ProfileService ProfileService { get; private set; }

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Инициализация сервисов
        AppFlow = GetComponent<AppFlowController>();
        if (AppFlow == null)
            Debug.LogError("AppFlowController не найден на AppBootstrap!");

        ProfileService = new ProfileService();
        ProfileService.LoadProfiles(); 
    }

    private void Start()
    {
        AppFlow?.SetState(AppState.MainMenu);
    }
}
