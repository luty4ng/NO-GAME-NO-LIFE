using System.Collections.Generic;
using UnityEngine;

public class KnightCharacter : Character
{
    public Sprite playerSprite;
    public Sprite knightSprite;

    public override void Action()
    {
        var phases = new List<Phase>();
        phases.Add(new Dialog("咱不废话了，直接开打吧。").SetRightImage(knightSprite));
        phases.Add(new Dialog("行。").SetLeftImage(playerSprite));
        phases.Add(new BattleEntry("是否接受战斗？", "Level 4").SetRightImage(knightSprite));
        MapGlobals.FeedDialog(phases);
    }
}