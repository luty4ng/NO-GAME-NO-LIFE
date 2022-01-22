using System.Collections.Generic;
using GameKit;
using UnityEngine;
using UnityEngine.Serialization;

public class Character : Interactable
{
    public Sprite playerSprite;
    public Sprite guardianSprite;
    
    public override void Action()
    {
        List<Phase> phases = new List<Phase>();
        phases.Add(new Dialog("第一条信息").SetRightImage(guardianSprite));
        phases.Add(new Dialog("第二条信息").SetLeftImage(playerSprite));
        phases.Add(new Dialog("第三条信息").SetRightImage(guardianSprite));
        EventManager.instance.EventTrigger<List<Phase>>(EventConfig.SHOW_DIALOG, phases);
    }
}