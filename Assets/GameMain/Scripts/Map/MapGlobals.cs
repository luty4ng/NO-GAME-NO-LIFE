using System.Collections.Generic;
using GameKit;

public class MapGlobals
{
    public static bool DialogUIActive = false;

    public static void FeedDialog(List<Phase> phases)
    {
        EventManager.instance.EventTrigger<List<Phase>>(EventConfig.SHOW_DIALOG, phases);
    }
}