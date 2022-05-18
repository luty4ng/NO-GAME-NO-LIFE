using System;
using System.Collections.Generic;
using UnityEngine;

public class PaperRecord1Item : SimpleDialogCharacter
{
    public Sprite paperRecordSprite;

    private Phase GetPaperRecordDialog(String message)
    {
        return new Dialog(message).SetRightImage(paperRecordSprite);
    }

    public override void AddDialogPhases(List<Phase> phases)
    {
        phases.Add(GetPaperRecordDialog("疾病诊断书："));
        phases.Add(GetPaperRecordDialog("\"年龄：17\""));
        phases.Add(GetPaperRecordDialog("\"职业：在读学生\""));
        phases.Add(GetPaperRecordDialog("\"测试结果：\""));
        phases.Add(GetPaperRecordDialog("\"总粗分 73\""));
        phases.Add(GetPaperRecordDialog("\"标准总分 91.25\""));
        phases.Add(GetPaperRecordDialog("\"参考诊断：（重度）抑郁\""));
        phases.Add(GetPaperRecordDialog("\"指导意见：\""));
        phases.Add(GetPaperRecordDialog("\"表现出注意力缺陷和嗜睡症状，自怨自责，兴趣缺乏。\""));
        phases.Add(GetPaperRecordDialog("\"社交单一，缺乏来自外部环境的支持。\""));
        phases.Add(GetPaperRecordDialog("\"因为多方压力和重要的家庭成员突然离开，患者情绪变得低落，并持续较长周期。\""));
    }
}