using System.Collections.Generic;
using UnityEngine;

public class DogCharacter : DialogCharacter
{
    public Sprite playerSprite;
    public Sprite dogSprite;

    public override void AddFirstTimePhases(List<Phase> phases)
    {
        phases.Add(new Dialog("嗷呜。").SetRightImage(dogSprite));
        phases.Add(new Dialog("你今天好像不太一样。").SetRightImage(dogSprite));
        phases.Add(new Dialog("你想要出去？ 你以前可一直都不太乐意出门。").SetRightImage(dogSprite));
        phases.Add(new Dialog("外面其实没什么好的...而且很危险。你就是从外面逃回来的，不记得了吗？").SetRightImage(dogSprite));
        phases.Add(new Dialog("呆在屋里的话，我还可以一直陪你。").SetRightImage(dogSprite));
        phases.Add(new Dialog("...（摇头）").SetLeftImage(playerSprite));
        phases.Add(new Dialog("你...一定要这样吗？").SetRightImage(dogSprite));
    }

    public override void AddRepeatPhases(List<Phase> phases)
    {
        phases.Add(new Dialog("...已经决定了吗？").SetRightImage(dogSprite));
    }

    public override void AddCommonEndPhases(List<Phase> phases)
    {
        phases.Add(new BattleEntry("是否要坚持出门？（接受战斗）", "Level 1").SetRightImage(dogSprite));
    }
}