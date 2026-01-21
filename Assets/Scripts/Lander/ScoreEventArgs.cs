using System;

public class ScoreEventArgs : EventArgs
{
    public int Score { get; private set; }

    public ScoreEventArgs(int score)
    {
            Score = score;
    }
}
