using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKit;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public sealed class UI_Counting : UIGroup
{
    private TextMeshProUGUI textMeshPro;
    private Sequence mySeq;
    protected override void Start()
    {
        UIManager.instance.RegisterUI(this as UIGroup);
        mySeq = DOTween.Sequence();
        float duration = MusicBattleRegulator.current.rhythmController.beatTravelTime / 4;
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        mySeq.Append(canvasGroup.DOFade(1, duration).OnComplete(() =>
        {
            textMeshPro.text = "2";
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, duration).OnComplete(() =>
            {
                textMeshPro.text = "1";
                canvasGroup.alpha = 0;
                canvasGroup.DOFade(1, duration).OnComplete(() =>
                {
                    textMeshPro.text = "0";
                    canvasGroup.alpha = 0;
                    canvasGroup.DOFade(1, duration).OnComplete(() =>
                    {
                        textMeshPro.text = "GO";
                        canvasGroup.alpha = 0;
                        canvasGroup.DOFade(1, duration).OnComplete(() =>
                        {
                            canvasGroup.alpha = 0;
                        });
                    });
                });
            });
        }));
    }

    private void Update()
    {
        if(Timer.isPause)
            mySeq.Pause();
        else
            mySeq.Play();
    }
}
