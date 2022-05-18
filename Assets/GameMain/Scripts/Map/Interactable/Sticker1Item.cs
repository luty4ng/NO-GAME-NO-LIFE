using System;
using System.Collections.Generic;
using UnityEngine;

public class Sticker1Item : SimpleDialogCharacter
{
    public Sprite stickerSprite;

    private Phase GetStickerDialog(String message)
    {
        return new Dialog(message).SetRightImage(stickerSprite);
    }

    public override void AddDialogPhases(List<Phase> phases)
    {
        phases.Add(GetStickerDialog("冰箱上的便签条："));
        phases.Add(GetStickerDialog("便签 1：“中午十二点和晚上九点记得吃药 白色一次三粒 橙色一次一粒“"));
        phases.Add(GetStickerDialog("便签 2：“吃了能对好心情有帮助的食物：巧克力 黄豆 杏仁 鸡蛋 不爱吃黄豆 多买杏仁”"));
        phases.Add(GetStickerDialog("便签 3：“心理医生的预约电话：217 *** **** ，别对孩子发火”"));
    }
}