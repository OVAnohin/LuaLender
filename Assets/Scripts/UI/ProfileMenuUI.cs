using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfileMenuUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    //[SerializeField] private Button createButton;
    //[SerializeField] private Button selectButton;
    //[SerializeField] private Button deleteButton;
    [SerializeField] private CanvasGroup canvasGroup;

    private ProfileService _profileService;

    private void Awake()
    {
        closeButton.onClick.AddListener(OnCloseClicked);
        //createButton.onClick.AddListener(OnCreateClicked);
        //selectButton.onClick.AddListener(OnSelectClicked);
        Hide();
    }

    public void Initialize(ProfileService profileService)
    {
        _profileService = profileService;
    }

    private void OnCreateClicked()
    {
        Debug.Log("тут открою новое окно, в котором будет ввод нового игрока");
    }

    private void OnSelectClicked()
    {
        Debug.Log("тут открою новое окно, в котором будет ввод нового игрока");
    }

    private void OnCloseClicked()
    {
        Hide();
    }

    public void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
