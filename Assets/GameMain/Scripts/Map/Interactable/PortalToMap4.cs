using GameKit;

public class PortalToMap4 : Portal
{
    public override void OnEnter()
    {
        if (MapGlobals.CurrentLevel < 3)
        {
            return;
        }
        base.OnEnter();
        string switchTo;
        if (MapGlobals.CurrentLevel < 4)
        {
            switchTo = "Map 4 Inner";
        }
        else
        {
            switchTo = "Map 4 Outer";
        }
        Scheduler.instance.SwitchSceneSwipe(switchTo);
    }
}