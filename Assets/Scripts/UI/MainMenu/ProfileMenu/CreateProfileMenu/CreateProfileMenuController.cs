using System;
using UnityEngine;

public class CreateProfileMenuController : MonoBehaviour
{
    [SerializeField] private CreateProfileMenuUI createProfileMenuUI;
    [SerializeField] private ProfileMenuController profileMenuController;

    public event EventHandler ShowWindow;
    public event EventHandler HideWindow;

    private void Awake()
    {
        if (createProfileMenuUI.gameObject.activeSelf == false)
            createProfileMenuUI.gameObject.SetActive(true);

        createProfileMenuUI.Initialize(this);
    }

    private void OnEnable()
    {
        profileMenuController.CreateNewProfileMenuClicked += OnCreateNewProfileMenuClicked;
    }

    private void OnDisable()
    {
        profileMenuController.CreateNewProfileMenuClicked -= OnCreateNewProfileMenuClicked;
    }

    private void OnCreateNewProfileMenuClicked(object sender, EventArgs e)
    {
        ShowWindow?.Invoke(this, EventArgs.Empty);
    }

    internal void OnCloseCreateProfileMenuClicked()
    {
        HideWindow?.Invoke(this, EventArgs.Empty);
    }

    internal void OnOkClicked(string text)
    {
        if (!System.String.IsNullOrEmpty(text))
        {
            Debug.Log(text);
        }

        HideWindow?.Invoke(this, EventArgs.Empty);

        ProfileService profileService = AppBootstrap.Instance.ProfileService;
        profileService.CreateProfile(text);
    }

    private void OnDestroy()
    {
        createProfileMenuUI.Deinitialize();
    }
}
