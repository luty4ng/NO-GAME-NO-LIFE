using UnityEngine;

public class Portal : Interactable
{
    public string sceneName;
    
    public override void Action()
    {
        MapRegulator.current.SwitchSceneSwipe(sceneName);
    }
}