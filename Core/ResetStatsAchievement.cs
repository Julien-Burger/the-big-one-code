using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStatsAchievement : MonoBehaviour
{
    [SerializeField] bool resetStats;
    [SerializeField] bool alsoResetAchievements;

    private void Start()
    {
        if (SteamManager.Initialized)
        {
            if (resetStats)
            {
                SteamUserStats.ResetAllStats(alsoResetAchievements);
            }
        }
    }
}
