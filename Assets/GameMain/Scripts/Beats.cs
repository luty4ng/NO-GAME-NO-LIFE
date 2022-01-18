using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameKit;
using SonicBloom.Koreo;

public enum BeatType
{
    Attack,
    Defense
}
[RequireComponent(typeof(Image))]
public class Beats : MonoBehaviour
{
    public BeatType beatType;
    Image visual;
    KoreographyEvent trackedEvent;
    TrackController trackController;
    RhythmController rhythmController;
    Sequence tweenSeq;
    RectTransform rectTransform;
    float offsetWidth = 0;
    private void Start()
    {
        visual = GetComponent<Image>();
    }
    #region Methods
    public void Initialize(KoreographyEvent evt, TrackController trackCtrl, RhythmController rhythmCtrl)
    {
        trackedEvent = evt;
        trackController = trackCtrl;
        rhythmController = rhythmCtrl;

        rectTransform = this.GetComponent<RectTransform>();
        rectTransform.SetParent(trackCtrl.transform);
        rectTransform.anchoredPosition = trackCtrl.spawn.anchoredPosition;
    }

    public void SetWidth(float diff)
    {
        float snap = (float)(diff / rhythmController.SamplesPerBeat);
        float speed = (trackController.spawn.anchoredPosition.x - trackController.hitter.anchoredPosition.x) / rhythmController.beatTravelTime;
        float disPerSnap = (float)(speed * rhythmController.TimePerSnap);
        rectTransform.sizeDelta = new Vector2(snap * disPerSnap, rectTransform.sizeDelta.y);
        offsetWidth = rectTransform.sizeDelta.x;
    }

    public void SetInitialMovement()
    {
        tweenSeq = DOTween.Sequence();
        float targetPosX = trackController.hitter.anchoredPosition.x - offsetWidth;
        float dis = Mathf.Abs(trackController.spawn.anchoredPosition.x - trackController.hitter.anchoredPosition.x);
        float moveTime = rhythmController.beatTravelTime + rhythmController.beatTravelTime * (offsetWidth / dis);
        // Debug.Log();
        tweenSeq.Append(rectTransform.DOLocalMoveX(targetPosX, moveTime).SetEase(Ease.Linear).OnComplete(() =>
         {
             trackController.isSpreaking = false;
             rectTransform.DOLocalMoveX(targetPosX * 1.1f, moveTime * 0.1f).OnComplete(() =>
             {
                 SelfDestroy();
             });
         }));

        EventManager.instance.AddEventListener<bool>(EventConfig.Game_Pase, Pause);
    }

    void Reset()
    {
        trackedEvent = null;
        trackController = null;
        rhythmController = null;
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


    public bool IsBeatHittable()
    {
        int beatTime = trackedEvent.StartSample;
        int curTime = rhythmController.DelayedSampleTime;
        int hitWindow = rhythmController.HitWindowSampleWidth;
        return (Mathf.Abs(beatTime - curTime) <= hitWindow);
    }

    public bool IsBeatSpreakable()
    {
        if (trackedEvent.IsOneOff())
            return false;
        int startTime = trackedEvent.StartSample;
        int endTime = trackedEvent.EndSample;
        int curTime = rhythmController.DelayedSampleTime;
        int hitWindow = rhythmController.HitWindowSampleWidth;
        return (Mathf.Abs(startTime - curTime) <= hitWindow);
    }

    public bool IsBeatMissed()
    {
        bool bMissed = true;
        if (enabled)
        {
            int beatTime = trackedEvent.StartSample;
            int curTime = rhythmController.DelayedSampleTime;
            int hitWindow = rhythmController.HitWindowSampleWidth;
            bMissed = (curTime - beatTime > hitWindow);
        }
        return bMissed;
    }

    void SelfDestroy()
    {
        tweenSeq.Kill();
        tweenSeq = null;
        Destroy(this.gameObject);
    }

    public void OnHit()
    {
        if (beatType == BeatType.Attack)
        {
            EventManager.instance.EventTrigger(EventConfig.P_Attack);
            EventManager.instance.EventTrigger(EventConfig.E_BeAttacked);
        }
        else if (beatType == BeatType.Defense)
        {
            EventManager.instance.EventTrigger(EventConfig.E_Attack);
            EventManager.instance.EventTrigger(EventConfig.P_Defense);
        }
        SetHitted();
        // SelfDestroy();
    }

    public void OnStreakEnter()
    {
        Debug.Log("OnStreakEnter");
        SetHitted();
        if (beatType == BeatType.Attack)
        {
            EventManager.instance.EventTrigger(EventConfig.P_Streak);
            EventManager.instance.EventTrigger(EventConfig.E_BeAttacked);
        }
        else if (beatType == BeatType.Defense)
        {
            EventManager.instance.EventTrigger(EventConfig.E_Streak);
            EventManager.instance.EventTrigger(EventConfig.P_Defense);
        }
    }
    public void OnStreakExit()
    {
        Debug.Log("OnStreakExit");
    }

    public void OnStreakUpdate()
    {
        Debug.Log("OnStreakUpdate");
    }
    private void SetHitted() => visual.color = Color.Lerp(visual.color, Color.black, 0.3f);

    public void OnMiss()
    {
        Debug.Log("IsMissing");
        if (beatType == BeatType.Attack)
            return;
        else if (beatType == BeatType.Defense)
        {
            EventManager.instance.EventTrigger(EventConfig.E_Attack);
            EventManager.instance.EventTrigger(EventConfig.P_BeAttacked);
        }
    }

    private void CheckAccuracy()
    {


    }
    public void OnClear()
    {
        SelfDestroy();
    }

    #endregion
}