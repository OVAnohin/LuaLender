using System;

public partial class Lander
{
    public class LanderScoreCalculatedEventArgs : EventArgs
    {
        public int Score { get; }
        public float LandingAngle { get; }
        public float LandingSpeed { get; }
        public LandingType LandingType { get; }

        public LanderScoreCalculatedEventArgs(int score, float landingAngle, float landingSpeed, LandingType landingType)
        {
            Score = score;
            LandingAngle = landingAngle;
            LandingSpeed = landingSpeed;
            LandingType = landingType;
        }
    }
}
