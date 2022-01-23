using GameKit;
using UnityEngine;

public class PortalToEnd : Character
{
    private bool _used = false;
    public override void Action()
    {
        if (_used) return;
        _used = true;
        GetComponent<AudioSource>()?.Play();
        MapRegulator.current.DialogIn();
        EventManager.instance.EventTrigger(EventConfig.SHOW_END_CREDIT);
    }
}