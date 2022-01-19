using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKit;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public sealed class UI_Accuracy : UIGroup
{
    private TextMeshProUGUI textMeshPro;
    private RectTransform tmpRectTransform;
    private Sequence mySeq;
    public Color missingColor;
    public Color goodColor;
    public Color perfectColor;
    protected override void Start()
    {
        UIManager.instance.RegisterUI(this as UIGroup);
        mySeq = DOTween.Sequence();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        tmpRectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void ShowAccuracy(string accuracy)
    {
        mySeq.Kill();
        mySeq = DOTween.Sequence();
        Color tmpColor = missingColor;
        if (accuracy == "Missing")
            tmpColor = missingColor;
        else if (accuracy == "Good")
            tmpColor = goodColor;
        else if (accuracy == "Perfect")
            tmpColor = perfectColor;
        textMeshPro.color = tmpColor;
        textMeshPro.text = accuracy;
        canvasGroup.alpha = 0;
        tmpRectTransform.localScale = Vector3.one;
        mySeq.Append(canvasGroup.DOFade(1, 0.1f)).Insert(0, tmpRectTransform.DOScale(Vector3.one * 1.2f, 0.1f).SetEase(Ease.OutBounce));
    }
}
