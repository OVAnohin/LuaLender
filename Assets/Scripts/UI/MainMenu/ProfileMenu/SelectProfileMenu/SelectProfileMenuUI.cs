using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectProfileMenuUI : MonoBehaviour
{
    [SerializeField] private Button okButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button[] profiles;

    public int NumberOfProfiles => _numberOfProfiles;
    public int SectctedIndex => _selectedIndex;

    private SelectProfileMenuController _selectProfileMenuController;
    private CanvasGroup _canvasGroup;
    private int _selectedIndex;
    private int _numberOfProfiles;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _numberOfProfiles = profiles.Length;
        _selectedIndex = -1;
        for (int i = 0; i < profiles.Length; i++)
        {
            int index = i;
            profiles[i].onClick.AddListener(() => Select(index));
        }
    }

    private void Select(int index)
    {
        _selectedIndex = index;

        for (int i = 0; i < profiles.Length; i++)
            profiles[i].interactable = i != index;
    }

    internal void Initialize(SelectProfileMenuController selectProfileMenuController)
    {
        _selectProfileMenuController = selectProfileMenuController;

        okButton.onClick.AddListener(_selectProfileMenuController.OnOkClicked);
        closeButton.onClick.AddListener(_selectProfileMenuController.OnCloseClicked);
        upButton.onClick.AddListener(_selectProfileMenuController.OnUpClicked);
        downButton.onClick.AddListener(_selectProfileMenuController.OnDownClicked);

        Subscribe();
    }

    private void Subscribe()
    {
        _selectProfileMenuController.ShowWindow += ShowWindow;
        _selectProfileMenuController.HideWindow += HideWindow;
        _selectProfileMenuController.CurrentProfileListUpdated += CurrentProfileListUpdated;
    }

    public void Deinitialize()
    {
        if (_selectProfileMenuController == null)
            return;

        _selectProfileMenuController.ShowWindow -= ShowWindow;
        _selectProfileMenuController.HideWindow -= HideWindow;
        _selectProfileMenuController.CurrentProfileListUpdated -= CurrentProfileListUpdated;
    }

    private void Start()
    {
        HideWindow(null, null);
    }

    private void CurrentProfileListUpdated(object sender, List<ProfileMenuEventArgs> profileArgs)
    {
        for (int i = 0; i < profiles.Length; i++)
        {
            TextMeshProUGUI textMeshPro = profiles[i].gameObject.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.text = profileArgs[i].PlayerName;
            profiles[i].interactable = true;
            _selectedIndex = -1;

            if (profileArgs[i].ProfileId.Equals("Zero"))
                profiles[i].enabled = false;
        }
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
