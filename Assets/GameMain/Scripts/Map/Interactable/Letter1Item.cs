using System;
using System.Collections.Generic;
using GameKit;
using UnityEngine;

public class Letter1Item : SimpleDialogCharacter
{
    public Sprite letterSprite;

    private Phase GetLetterDialog(String message)
    {
        Phase phase = new Dialog(message);
        if (letterSprite != null)
        {
            phase = phase.SetRightImage(letterSprite);
        }
        return phase;
    }

    public override void AddDialogPhases(List<Phase> phases)
    {
        phases.Add(GetLetterDialog("来自朋友的信息："));
        phases.Add(GetLetterDialog("\"你还好吗？最近感觉好点了吗？\""));
        phases.Add(GetLetterDialog("\"有一段时间没有见到你啦，我们都很想念你。\""));
        phases.Add(GetLetterDialog("\"我们都知道比尔对你的意义，它陪你一起长大，我知道这件事的时候也非常非常难过。\""));
        phases.Add(GetLetterDialog("\"希望你能快点好起来，尽管你需要时间才能走出来。\""));
        phases.Add(GetLetterDialog("\"比尔也一定会比我更希望你振作起来的，对吧？\""));
        phases.Add(GetLetterDialog("\"学校的作业，材料我都帮你整理好放在你桌上了。\""));
        phases.Add(GetLetterDialog("\"有什么不懂的课题也可以找我，我愿意讲给你。\""));
        phases.Add(GetLetterDialog("\"今天是放假前最后一天，下午学校来拿一下这些作业，好吗？\""));
    }
}