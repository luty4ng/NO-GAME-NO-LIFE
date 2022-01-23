using System;
using System.Collections.Generic;
using GameKit;
using UnityEngine;

public class MapGlobals
{
    private static readonly string GAME_SAVE_LOCATION = "GameSave";
    private static readonly string CURRENT_LEVEL_KEY = "CurrentLevel";
    private static readonly int DEFAULT_LEVEL = 0;  // the level that the player have went through (0-4)
    
    public static bool DialogUIActive = false;

    public static void FeedDialog(List<Phase> phases)
    {
        EventManager.instance.EventTrigger<List<Phase>>(EventConfig.SHOW_DIALOG, phases);
    }

    public static bool HasSave => JsonManager.instance.CheckJsonExist(GAME_SAVE_LOCATION);

    private static int _currentLevelCache = -1;

    public static int CurrentLevel
    {
        get
        {
            if (_currentLevelCache == -1)
            {
                if (HasSave)
                {
                    var json = JsonManager.instance.LoadJsonDict<string, int>(GAME_SAVE_LOCATION);
                    _currentLevelCache = json[CURRENT_LEVEL_KEY];
                }
                else
                {
                    CurrentLevel = DEFAULT_LEVEL;
                }
            }

            return _currentLevelCache;
        }
        set
        {
            JsonManager.instance.SaveJsonDict(GAME_SAVE_LOCATION, new Dictionary<string, int>()
            {
                {CURRENT_LEVEL_KEY, value}
            });
            _currentLevelCache = value;
        }
    }

    public static void SwitchToStartScene()
    {
        string switchTo;
        switch (CurrentLevel)
        {
            case 0: switchTo = "Map 1 Inner"; break;
            case 1: switchTo = "Map 1 Outer"; break;
            case 2: switchTo = "Map 2 Outer"; break;
            case 3: switchTo = "Map 3 Outer"; break;
            case 4: switchTo = "Map 4 Outer"; break;
            default: throw new ApplicationException("Invalid level code.");
        }
        Scheduler.instance.SwitchSceneSwipe(switchTo);
    }
}