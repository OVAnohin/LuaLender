using System;

public class LanderEventArgs : EventArgs
{
    private Lander _lander;

    public Lander Lander
    {
        get { return _lander; }
    }


    public LanderEventArgs(Lander lander)
    {
        _lander = lander;
    }
}
