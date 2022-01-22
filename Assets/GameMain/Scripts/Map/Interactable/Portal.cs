using GameKit;
using UnityEngine;

public class Portal : Interactable
{
    public string sceneName;
    
    public override void OnEnter()
    {
        Scheduler.instance.SwitchSceneSwipe(sceneName);
    }
}