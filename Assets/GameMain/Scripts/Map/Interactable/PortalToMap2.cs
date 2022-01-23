using GameKit;

public class PortalToMap2 : Portal
{
    public override void OnEnter()
    {
        base.OnEnter();
        string switchTo;
        if (MapGlobals.CurrentLevel < 2)
        {
            switchTo = "Map 2 Inner";
        }
        else
        {
            switchTo = "Map 2 Outer";
        }
        Scheduler.instance.SwitchSceneSwipe(switchTo);
    }
}
