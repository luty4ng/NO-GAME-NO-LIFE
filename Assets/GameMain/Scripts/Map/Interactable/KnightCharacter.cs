using System.Collections.Generic;
using GameKit;
using UnityEngine;

public class KnightCharacter : DialogCharacter
{
    public Sprite playerSprite;
    public Sprite knightSprite;
    
    private void Start()
    {
        if (MapGlobals.CurrentLevel == 4)
        {
            var phases = new List<Phase>();
            phases.Add(new Dialog("很不愿意承认，你变得不一样了。").SetRightImage(knightSprite));
            phases.Add(new Dialog("你打败了我。").SetRightImage(knightSprite));
            phases.Add(new Dialog("你知道出去之后你要面对什么。").SetRightImage(knightSprite));
            phases.Add(new Dialog("即便这样，你还是要走吗？").SetRightImage(knightSprite));
            phases.Add(new Dialog("...").SetRightImage(knightSprite));
            phases.Add(new Dialog("好吧。如果你已经准备好接受一切、面对一切的话，我会和你一起。").SetRightImage(knightSprite));
            phases.Add(new Dialog("我真羡慕你的勇气啊。").SetRightImage(knightSprite));
            phases.Add(new Dialog("出门去吧。").SetRightImage(knightSprite).SetCallback(() =>
            {
                Scheduler.instance.SwitchSceneSwipe("Map 4 Outer");
            }));
            MapGlobals.FeedDialog(phases);
        }
    }

    public override void AddFirstTimePhases(List<Phase> phases)
    {
        phases.Add(new Dialog("你为什么在这里？回去你的房间，那里是最安全的，也是你该呆的地方。").SetRightImage(knightSprite));
        phases.Add(new Dialog("你知道外面比这里更危险。那里都是陌生的环境，没缘由的恶意，也没有人陪伴你！").SetRightImage(knightSprite));
        phases.Add(new Dialog("我不允许——我不会让你再踏出一步！").SetRightImage(knightSprite));
        phases.Add(new Dialog("你要证明你自己？").SetRightImage(knightSprite));
        phases.Add(new Dialog("你真的觉得你能够赢过我？").SetRightImage(knightSprite));
    }

    public override void AddRepeatPhases(List<Phase> phases)
    {
        phases.Add(new Dialog("你要妄想挑战我？").SetRightImage(knightSprite));
    }

    public override void AddCommonEndPhases(List<Phase> phases)
    {
        phases.Add(new BattleEntry("从门前让开！（接受战斗）", "Level 4").SetRightImage(knightSprite));
    }
}