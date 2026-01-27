using System;

[Serializable]
public class PlayerStatistics
{
    public int TotalScore;
    public int LevelsPlayed;
    public int LevelsWin;
    public int LevelsLose;
    public float TotalPlayTime;

    public void RegisterWin(int score, float playTime)
    {
        LevelsPlayed++;
        LevelsWin++;
        TotalScore += score;
        TotalPlayTime += playTime;
    }

    public void RegisterLose(float playTime)
    {
        LevelsPlayed++;
        LevelsLose++;
        TotalPlayTime += playTime;
    }
}
