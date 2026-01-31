using System;

public class ProfileMenuEventArgs : EventArgs
{
    public string ProfileId { get; }
    public string PlayerName { get; }
    public int Index { get; }

    public ProfileMenuEventArgs(string profileId, string playerName, int index)
    {
        ProfileId = profileId;
        PlayerName = playerName;
        Index = index;
    }
}
