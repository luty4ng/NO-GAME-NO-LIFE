using GameKit;

public class PortalToMap3 : Character
{
    public override void Action()
    {
        string switchTo;
        if (MapGlobals.CurrentLevel < 3)
        {
            switchTo = "Map 3 Inner";
        }
        else
        {
            switchTo = "Map 3 Outer";
        }
        Scheduler.instance.SwitchSceneSwipe(switchTo);
    }
}