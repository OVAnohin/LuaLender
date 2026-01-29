using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectProfileMenuUI : MonoBehaviour
{
    [SerializeField] private Button okButton;
    [SerializeField] private Button closeButton;

    private SelectProfileMenuController _selectProfileMenuController;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    internal void Initialize(SelectProfileMenuController selectProfileMenuController)
    {
        _selectProfileMenuController = selectProfileMenuController;

        okButton.onClick.AddListener(_selectProfileMenuController.OnOkClicked);
        closeButton.onClick.AddListener(_selectProfileMenuController.OnCloseSelectProfileMenuClicked);

        Subscribe();
    }

    private void Subscribe()
    {
        _selectProfileMenuController.ShowWindow += ShowWindow;
        _selectProfileMenuController.HideWindow += HideWindow;
    }

    public void Deinitialize()
    {
        if (_selectProfileMenuController == null)
            return;

        _selectProfileMenuController.ShowWindow -= ShowWindow;
        _selectProfileMenuController.HideWindow -= HideWindow;
    }

    private void Start()
    {
        HideWindow(null, null);
    }

    private void ShowWindow(object sender, System.EventArgs e)
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    private void HideWindow(object sender, System.EventArgs e)
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
