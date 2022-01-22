using System.Collections.Generic;
using UnityEngine;

public class MonsterCharacter : Character
{
    public Sprite playerSprite;
    public Sprite monsterSprite;

    public override void Action()
    {
        var phases = new List<Phase>();
        phases.Add(new Dialog("人 这不是你该来的地方。").SetRightImage(monsterSprite));
        phases.Add(new Dialog("我就要来。").SetLeftImage(playerSprite));
        phases.Add(new Dialog("那就只能打了。").SetRightImage(monsterSprite));
        phases.Add(new BattleEntry("是否接受战斗？", "Level 2").SetRightImage(monsterSprite));
        MapGlobals.FeedDialog(phases);
    }
}