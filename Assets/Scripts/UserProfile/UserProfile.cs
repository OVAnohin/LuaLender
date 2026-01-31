using System;

[Serializable]
public class UserProfile
{
    public string ProfileId;
    public PlayerInfo PlayerInfo;
    public PlayerStatistics Statistics;
    public PlayerProgress Progress;
    public ProfileMeta Meta;

    public UserProfile(string playerName, string avatarId = null)
    {
        ProfileId = Guid.NewGuid().ToString();

        PlayerInfo = new PlayerInfo
        {
            PlayerName = playerName,
            AvatarId = avatarId
        };

        Statistics = new PlayerStatistics();
        Progress = new PlayerProgress();
        Meta = new ProfileMeta
        {
            CreatedAt = DateTime.UtcNow,
            LastPlayedAt = DateTime.UtcNow
        };
    }
}
