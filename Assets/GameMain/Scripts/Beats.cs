using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameKit;
using SonicBloom.Koreo;

public class Beats : MonoBehaviour
{

    KoreographyEvent trackedEvent;
    TrackController trackController;
    RhythmController rhythmController;
    Sequence tweenSeq;
    RectTransform rectTransform;
    #region Methods
    public void Initialize(KoreographyEvent evt, TrackController trackCtrl, RhythmController rhythmCtrl)
    {
        trackedEvent = evt;
        trackController = trackCtrl;
        rhythmController = rhythmCtrl;

        rectTransform = this.GetComponent<RectTransform>();
        rectTransform.SetParent(trackCtrl.transform);
        rectTransform.anchoredPosition = trackCtrl.spawn.anchoredPosition;

        tweenSeq = DOTween.Sequence();
        tweenSeq.Append(rectTransform.DOLocalMoveX(trackCtrl.hitter.anchoredPosition.x, rhythmCtrl.beatTravelTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            rectTransform.DOLocalMoveX(trackCtrl.hitter.anchoredPosition.x * 1.1f, rhythmCtrl.beatTravelTime * 0.1f).OnComplete(() =>
            {
                SelfDestroy();
            });
        }));

    }

    void Reset()
    {
        trackedEvent = null;
        trackController = null;
        rhythmController = null;
    }
    public bool IsBeatHittable()
    {
        int noteTime = trackedEvent.StartSample;
        int curTime = rhythmController.DelayedSampleTime;
        int hitWindow = rhythmController.HitWindowSampleWidth;
        return (Mathf.Abs(noteTime - curTime) <= hitWindow);
    }

    public bool IsBeatMissed()
    {
        bool bMissed = true;

        if (enabled)
        {
            int noteTime = trackedEvent.StartSample;
            int curTime = rhythmController.DelayedSampleTime;
            int hitWindow = rhythmController.HitWindowSampleWidth;

            bMissed = (curTime - noteTime > hitWindow);
        }
        return bMissed;
    }
    void ReturnToPool()
    {
        tweenSeq.Kill();
        tweenSeq = null;
        rhythmController.ReturnBeatsToPool(this);
        Reset();
    }

    void SelfDestroy()
    {
        tweenSeq.Kill();
        tweenSeq = null;
        Destroy(this.gameObject);
    }

    public void OnHit()
    {
        Debug.Log("Hit");
        SelfDestroy();
    }
    public void OnClear()
    {
        SelfDestroy();
    }

    #endregion
}