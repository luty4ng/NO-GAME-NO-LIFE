using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKit;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public sealed class UI_Accuracy_Mono : MonoBehaviour
{
    private Image image;
    private RectTransform tmpRectTransform;
    private Sequence mySeq;
    public Sprite missing;
    public Sprite good;
    public Sprite perfect;
    private CanvasGroup canvasGroup;
    private void Start()
    {
        mySeq = DOTween.Sequence();
        image = GetComponentInChildren<Image>();
        tmpRectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void ShowAccuracy(string accuracy)
    {
        mySeq.Kill();
        mySeq = DOTween.Sequence();
        Sprite tmpSprite = missing;
        if (accuracy == "Missing")
            tmpSprite = missing;
        else if (accuracy == "Good")
            tmpSprite = good;
        else if (accuracy == "Perfect")
            tmpSprite = perfect;
        image.sprite = tmpSprite;
        canvasGroup.alpha = 0;
        tmpRectTransform.localScale = Vector3.one;
        mySeq.Append(canvasGroup.DOFade(1, 0.1f)).Insert(0, tmpRectTransform.DOScale(Vector3.one * 1.2f, 0.1f).SetEase(Ease.OutBounce));
    }
}
