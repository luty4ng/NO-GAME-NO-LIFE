using System.Collections.Generic;
using UnityEngine;

public class GuardianCharacter : DialogCharacter
{
    public Sprite playerSprite;
    public Sprite guardianSprite;

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