using System;
using System.Collections.Generic;

[Serializable]
public class UserProfilesData
{
    public List<UserProfile> Profiles = new List<UserProfile>();
    public string ActiveProfileId;
}
