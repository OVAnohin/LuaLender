using System;

public class LanderArgs : EventArgs
{
    private Lander _lander;

    public Lander Lander
    {
        get { return _lander; }
    }


    public LanderArgs(Lander lander)
    {
        _lander = lander;
    }
}
