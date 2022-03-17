using System;
using System.Collections.Generic;
using GameKit;
using UnityEngine;

public class MapGlobals
{
    public static bool GamePaused = false;
    
    private static readonly string GAME_SAVE_LOCATION = "GameSave";
    private static readonly string CURRENT_LEVEL_KEY = "CurrentLevel";
    private static readonly int DEFAULT_LEVEL = 0;  // the level that the player have went through (0-4)

    public static void FeedDialog(List<Phase> phases)
    {
        EventManager.instance.EventTrigger<List<Phase>>(EventConfig.SHOW_DIALOG, phases);
    }

    public static bool HasSave
    {
        get
        {
            if (!JsonManager.instance.CheckJsonExist(GAME_SAVE_LOCATION)) return false;
            return JsonManager.instance.LoadJsonDict<string, int>(GAME_SAVE_LOCATION)[CURRENT_LEVEL_KEY] >= 0;
        }
    }

    private static int _currentLevelCache = -1;

    public static int CurrentLevel
    {
        get
        {
            if (_currentLevelCache == -1 && HasSave)
            {
                var json = JsonManager.instance.LoadJsonDict<string, int>(GAME_SAVE_LOCATION);
                _currentLevelCache = json[CURRENT_LEVEL_KEY];
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

    public static void SwitchToSceneUsingSave()
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
    
    public static void SwitchToStartSceneAndOverrideSave()
    {
        CurrentLevel = -1;
        Scheduler.instance.SwitchSceneSwipe("Map 1 Inner");
    }

    public static void SwitchToMain()
    {
        Scheduler.instance.SwitchSceneSwipe("S_Menu_New");
    }
}