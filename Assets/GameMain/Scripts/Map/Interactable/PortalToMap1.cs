using GameKit;
using UnityEngine;

public class PortalToMap1 : Portal
{
    public override void OnEnter()
    {
        Debug.Log("Portal to map 1 triggered");
        base.OnEnter();
        string switchTo;
        if (MapGlobals.CurrentLevel < 1)
        {
            switchTo = "Map 1 Inner";
        }
        else
        {
            switchTo = "Map 1 Outer";
        }
        Scheduler.instance.SwitchSceneSwipe(switchTo);
    }
}
