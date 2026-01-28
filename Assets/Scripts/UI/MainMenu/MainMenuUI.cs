using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button profileButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private MainMenuController _controller;

    public void Initialize(MainMenuController controller)
    {
        _controller = controller;

        playButton.onClick.AddListener(_controller.OnPlayClicked);
        profileButton.onClick.AddListener(_controller.OnProfileClicked);
        settingsButton.onClick.AddListener(_controller.OnSettingsClicked);
        quitButton.onClick.AddListener(_controller.OnQuitClicked);
    }

    public void UpdatePlayButtonState(bool isInteractable)
    {
        playButton.interactable = isInteractable;
    }
}
