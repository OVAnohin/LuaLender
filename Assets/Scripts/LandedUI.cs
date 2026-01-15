using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TitleTextMesh;
    [SerializeField] private TextMeshProUGUI StatsTextMesh;
    [SerializeField] private Lander Lander;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        Hide();
    }

    private void OnEnable()
    {
        Lander.OnLanded += LanderOnLanded;
    }

    private void OnDisable()
    {
        Lander.OnLanded -= LanderOnLanded;
    }

    private void LanderOnLanded(object sender, Lander.LandingScoreCalculatedEventArgs args)
    {
        if (args.LandingType.Equals(Lander.LandingType.Success))
        {
            TitleTextMesh.text = "Successful Landing!";
        }
        else
        {
            TitleTextMesh.color = Color.red;
            TitleTextMesh.text = args.LandingType.ToString();
        }

        StatsTextMesh.text =
            args.LandingSpeed + "\n" +
            args.LandingAngle + "\n" +
            0 + "\n" +
            args.Score + "\n";

        StartCoroutine(PauseBeforeShow(.5f));
    }

    private IEnumerator PauseBeforeShow(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Show();
    }

    private void Show()
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
