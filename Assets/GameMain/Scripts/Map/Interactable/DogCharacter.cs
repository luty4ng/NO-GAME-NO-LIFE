using System;
using System.Collections.Generic;
using GameKit;
using UnityEngine;

public class DogCharacter : DialogCharacter
{
    public Sprite playerSprite;
    public Sprite dogSprite;

    private void Start()
    {
        if (MapGlobals.CurrentLevel == -1)
        {
            var phases = new List<Phase>();
            phases.Add(new Dialog("使用 A 和 D 键控制角色方向，使用 L 开启场景交互或对话翻页（按 L 继续）"));
            phases.Add(new Dialog("没错，就是这样。"));
            phases.Add(new Dialog("接下来你就可以开始你的冒险了，勇士！").SetCallback(() =>
            {
                MapGlobals.CurrentLevel = 0;
            }));
            MapGlobals.FeedDialog(phases);
        }
        if (MapGlobals.CurrentLevel == 1)
        {
            var phases = new List<Phase>();
            phases.Add(new Dialog("这才像你 ...这一天我等了很久很久。").SetRightImage(dogSprite));
            phases.Add(new Dialog("...比尔最后的任务已经完成。").SetRightImage(dogSprite));
            phases.Add(new Dialog("勇敢向前吧，你最棒啦。").SetRightImage(dogSprite));
            phases.Add(new Dialog("再见！呜汪！").SetRightImage(dogSprite).SetCallback(() =>
            {
                Scheduler.instance.SwitchSceneSwipe("Map 1 Outer");
            }));
            MapGlobals.FeedDialog(phases);
        }
    }

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