using System.Collections.Generic;
using GameKit;
using UnityEngine;

public class GuardianCharacter : DialogCharacter
{
    public Sprite playerSprite;
    public Sprite guardianSprite;
    
    private void Start()
    {
        if (MapGlobals.CurrentLevel == 3)
        {
            var phases = new List<Phase>();
            phases.Add(new Dialog("你不知道我们背负的重担，你只在乎你自己的感受...").SetRightImage(guardianSprite));
            phases.Add(new Dialog("还是说，没有考虑别人感受的擅自做出决定的，其实是我们？").SetRightImage(guardianSprite));
            phases.Add(new Dialog("你——长大了...").SetRightImage(guardianSprite));
            phases.Add(new Dialog("你可以为自己做决定了...").SetRightImage(guardianSprite).SetCallback(() =>
            {
                Scheduler.instance.SwitchSceneSwipe("Map 3 Outer");
            }));
            MapGlobals.FeedDialog(phases);
        }
    }

    public override void AddFirstTimePhases(List<Phase> phases)
    {
        phases.Add(new Dialog("你——不应该——在这里").SetRightImage(guardianSprite));
        phases.Add(new Dialog("禁止——通过——").SetRightImage(guardianSprite));
        phases.Add(new Dialog("必须——守护...").SetRightImage(guardianSprite));
    }

    public override void AddRepeatPhases(List<Phase> phases)
    {
        phases.Add(new Dialog("这———都是为了你好....").SetRightImage(guardianSprite));
    }

    public override void AddCommonEndPhases(List<Phase> phases)
    {
        phases.Add(new BattleEntry("让我过去！（接受战斗）", "Level 3").SetRightImage(guardianSprite));
    }
}