using System.Collections.Generic;
using UnityEngine;

public class MonsterCharacter : DialogCharacter
{
    public Sprite playerSprite;
    public Sprite monsterSprite;

    public override void AddFirstTimePhases(List<Phase> phases)
    {
        phases.Add(new Dialog("“哭丧着脸给谁看呢？我们维持这个家很累的！”").SetRightImage(monsterSprite));
        phases.Add(new Dialog("“同学你能不能至少把作业写完啊？”").SetRightImage(monsterSprite));
        phases.Add(new Dialog("”我要转学啦，因为搬家的事情 对不起这么晚才告诉你。...如果你积极一点的话，你会在会认识新的朋友的。”").SetRightImage(monsterSprite));
        phases.Add(new Dialog("“我们都知道它回不来！我们也很难过！”").SetRightImage(monsterSprite));
    }

    public override void AddRepeatPhases(List<Phase> phases)
    {
        phases.Add(new Dialog("“按时吃药，听医生的话，你会好起来的“").SetRightImage(monsterSprite));
    }

    public override void AddCommonEndPhases(List<Phase> phases)
    {
        phases.Add(new BattleEntry("我不想听了！（接受战斗）", "Level 2").SetRightImage(monsterSprite));
    }
}