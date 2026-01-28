using System;

[Serializable]
public class SaveData
{
    public int Score;

    public SaveData(int score = 0)
    {
        Score = score;
    }
}
