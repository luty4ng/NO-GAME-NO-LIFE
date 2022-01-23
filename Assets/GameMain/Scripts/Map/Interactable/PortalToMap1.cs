using GameKit;

public class PortalToMap1 : Portal
{
    public override void OnEnter()
    {
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
