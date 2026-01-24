using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Button ResetScoreButton;

    private void Awake()
    {
        PlayButton.onClick.AddListener(StartGame);
        QuitButton.onClick.AddListener(QuitGame);
        ResetScoreButton.onClick.AddListener(ResetScore);
    }

    private void ResetScore()
    {
        int score = 0;
        SaveService.Save(new SaveData(score));
    }

    private void StartGame()
    {
        SceneManager.LoadScene(SceneNames.GameScene.ToString());
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
