using System.Collections.Generic;
using UnityEngine;

public class GuardianCharacter : Character
{
    public Sprite playerSprite;
    public Sprite guardianSprite;

    public override void Action()
    {
        var phases = new List<Phase>();
        phases.Add(new Dialog("年轻人，外面的世界很危险，你确定你要出去吗？").SetRightImage(guardianSprite));
        phases.Add(new Dialog("我准备好了。").SetLeftImage(playerSprite));
        phases.Add(new Dialog("很好。在你出去之前，让我来与你比试一场。").SetRightImage(guardianSprite));
        phases.Add(new BattleEntry("是否接受战斗？", "Level 1").SetRightImage(guardianSprite));
        MapGlobals.FeedDialog(phases);
    }
}