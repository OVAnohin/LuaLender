using System;
using UnityEngine;

public class SelectProfileMenuController : MonoBehaviour
{
    [SerializeField] private SelectProfileMenuUI selectProfileMenuUI;
    [SerializeField] private ProfileMenuController profileMenuController;

    public event EventHandler ShowWindow;
    public event EventHandler HideWindow;

    private ProfileService _profileService;

    private void Awake()
    {
        _profileService = AppBootstrap.Instance.ProfileService;

        if (selectProfileMenuUI.gameObject.activeSelf == false)
            selectProfileMenuUI.gameObject.SetActive(true);

        selectProfileMenuUI.Initialize(this);
    }

    private void OnEnable()
    {
        profileMenuController.SelectProfileMenuClicked += SelectProfileMenuClicked;
    }

    private void OnDisable()
    {
        profileMenuController.SelectProfileMenuClicked -= SelectProfileMenuClicked;
    }

    private void SelectProfileMenuClicked(object sender, EventArgs e)
    {
        ShowWindow?.Invoke(this, EventArgs.Empty);
    }

    internal void OnCloseSelectProfileMenuClicked()
    {
        HideWindow?.Invoke(this, EventArgs.Empty);
    }

    public void OnOkClicked()
    {
        Debug.Log("OnOkClicked");
        HideWindow?.Invoke(this, EventArgs.Empty);

        //ProfileService profileService = AppBootstrap.Instance.ProfileService;
        //profileService.CreateProfile(text);
    }

    private void OnDestroy()
    {
        selectProfileMenuUI.Deinitialize();
    }
}
