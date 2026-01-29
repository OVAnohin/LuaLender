using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button profileButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI currentProfile;

    private MainMenuController _mainMenuController;

    public void Initialize(MainMenuController mainMenuController)
    {
        _mainMenuController = mainMenuController;

        playButton.onClick.AddListener(_mainMenuController.OnPlayClicked);
        profileButton.onClick.AddListener(_mainMenuController.OnProfileClicked);
        settingsButton.onClick.AddListener(_mainMenuController.OnSettingsClicked);
        quitButton.onClick.AddListener(_mainMenuController.OnQuitClicked);

        Subscribe();
    }

    private void Subscribe()
    {
        _mainMenuController.UpdatePlayButtonStatus += UpdatePlayButtonState;
        _mainMenuController.UpdateUserName += UpdateUserName;
    }

    public void Deinitialize()
    {
        if (_mainMenuController == null)
            return;

        _mainMenuController.UpdatePlayButtonStatus -= UpdatePlayButtonState;
        _mainMenuController.UpdateUserName -= UpdateUserName;
    }

    private void UpdateUserName(string userName)
    {
        currentProfile.text = userName;
    }

    private void UpdatePlayButtonState(bool isInteractable)
    {
        playButton.interactable = isInteractable;
    }
}
