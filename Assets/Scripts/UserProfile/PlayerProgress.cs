using System;
using System.Collections.Generic;

[Serializable]
public class PlayerProgress
{
    public List<int> UnlockedLevels = new List<int>();
    public List<string> Achievements = new List<string>();
}
