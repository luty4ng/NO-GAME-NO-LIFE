using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKit;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public sealed class UI_Counting : UIForm
{
    public TextMeshProUGUI textMeshPro;
    private Sequence mySeq;
    private CanvasGroup canvasGroup;
    protected override void Start()
    {
        mySeq = DOTween.Sequence();
        // Debug.Log(MusicBattleRegulator.current.rhythmController);
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
}
