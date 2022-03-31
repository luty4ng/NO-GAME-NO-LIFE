using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameKit;
using SonicBloom.Koreo;
using System.Collections;

[RequireComponent(typeof(Image))]
public class BeatC : MonoBehaviour
{
    KoreographyEvent kEvent;
    TrackC trackC;
    ParserC parserC;
    Sequence tweenSeq;
    RectTransform rectTransform;
    Animator animator;
    UI_Accuracy_Mono uI_Accuracy;
    float offsetWidth = 0;

    #region Methods
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Initialize(KoreographyEvent evt, TrackC tracker, ParserC rhythmCtrl)
    {
        kEvent = evt;
        trackC = tracker;
        parserC = rhythmCtrl;
        uI_Accuracy = parserC.uI_Accuracy;

        rectTransform = this.GetComponent<RectTransform>();
        rectTransform.SetParent(trackC.lane);
        rectTransform.anchoredPosition = tracker.spawn.anchoredPosition;
    }

    public void SetInitialMovement()
    {
        tweenSeq = DOTween.Sequence();
        float targetPosX = trackC.hitter.anchoredPosition.x - offsetWidth;
        float dis = Mathf.Abs(trackC.spawn.anchoredPosition.x - trackC.hitter.anchoredPosition.x);
        float moveTime = parserC.travelTime + parserC.travelTime * (offsetWidth / dis);
        EventManager.instance.AddEventListener<bool>(EventConfig.Game_Pase, Pause);
        tweenSeq.Append(rectTransform.DOLocalMoveX(targetPosX, moveTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            rectTransform.DOLocalMoveX(targetPosX * 1.5f, moveTime * 0.5f).OnComplete(() =>
            {
                tweenSeq.Kill();
                Destroy(this.gameObject);
            });
        }));


    }
    void Pause(bool isPause)
    {
        if (tweenSeq == null)
            return;

        if (isPause)
            tweenSeq.Pause();
        else
            tweenSeq.Play();
    }

    public bool IsEnter()
    {
        int startTime = kEvent.StartSample;
        int curTime = parserC.CurrentSampleTime;
        int hitWindow = parserC.HitWindowSampleWidth;
        return (Mathf.Abs(startTime - curTime) <= hitWindow);
    }

    public bool IsEnd()
    {
        int endTime = kEvent.EndSample;
        int curTime = parserC.CurrentSampleTime;
        int hitWindow = parserC.HitWindowSampleWidth;
        return (Mathf.Abs(endTime - curTime) <= hitWindow / 4);
    }

    public bool IsCenter()
    {
        int startTime = kEvent.StartSample;
        int curTime = parserC.CurrentSampleTime;
        int hitWindow = parserC.HitWindowSampleWidth;
        return (Mathf.Abs(startTime - curTime) <= hitWindow / 4);
    }
    public bool IsMiss()
    {
        int startTime = kEvent.StartSample;
        int curTime = parserC.CurrentSampleTime;
        int hitWindow = parserC.HitWindowSampleWidth;
        return (curTime - startTime > hitWindow);
    }

    public void OnHit()
    {
        Debug.Log("IsHit");
        animator.SetTrigger("Collapse");
        trackC.hitterAnimator.SetTrigger("Hit");
        CheckAccuracy();
    }

    public void OnMiss()
    {
        Debug.Log("IsMiss");
        uI_Accuracy.ShowAccuracy("Missing");
    }

    void CheckAccuracy()
    {
        int beatTime = kEvent.StartSample;
        int curTime = parserC.CurrentSampleTime;
        int perfectWindow = parserC.PerfectWindowSampleWidth;
        if (Mathf.Abs(beatTime - curTime) <= perfectWindow)
            uI_Accuracy.ShowAccuracy("Perfect");
        else
            uI_Accuracy.ShowAccuracy("Good");
    }
    public void OnClear()
    {
        Destroy(this.gameObject);
    }

    #endregion
}