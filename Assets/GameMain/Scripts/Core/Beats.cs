using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameKit;
using SonicBloom.Koreo;
using System.Collections;

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
    Animator animator;
    
    float offsetWidth = 0;
    float damageMultipier = 1;
    private bool IsStreak
    {
        get
        {
            return rectTransform.sizeDelta.x > 50;
        }
    }
    private void Start()
    {
        visual = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }
    #region Methods
    public void Initialize(KoreographyEvent evt, TrackController trackCtrl, RhythmController rhythmCtrl)
    {
        trackedEvent = evt;
        trackController = trackCtrl;
        rhythmController = rhythmCtrl;

        rectTransform = this.GetComponent<RectTransform>();
        rectTransform.SetParent(trackController.LaneMask.transform);
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
        tweenSeq.Append(rectTransform.DOLocalMoveX(targetPosX, moveTime).SetEase(Ease.Linear).OnComplete(() =>
         {
             trackController.isSpreaking = false;
             if (beatType == BeatType.Defense)
             {
                 if (IsStreak)
                     EventManager.instance.EventTrigger(EventConfig.E_Streak);
                 else
                     EventManager.instance.EventTrigger(EventConfig.E_Attack);
             }
             rectTransform.DOLocalMoveX(targetPosX * 1.5f, moveTime * 0.5f).OnComplete(() =>
             {
                 tweenSeq.Kill();
             });
         }));

        EventManager.instance.AddEventListener<bool>(EventConfig.Game_Pase, Pause);
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
        int beatTime = trackedEvent.StartSample;
        int curTime = rhythmController.DelayedSampleTime;
        int hitWindow = rhythmController.HitWindowSampleWidth;
        bMissed = (curTime - beatTime > hitWindow);
        return bMissed;
    }

    private bool IsBeatEnd()
    {
        int endTime = trackedEvent.EndSample;
        int curTime = rhythmController.DelayedSampleTime;
        int hitWindow = rhythmController.HitWindowSampleWidth;
        return (Mathf.Abs(endTime - curTime) <= hitWindow);
    }

    public void OnHit()
    {
        OnHitted();
        CheckAccuracy();
        if (!IsStreak)
            animator.SetTrigger("Collapse");
        

        if (beatType == BeatType.Attack)
        {
            EventManager.instance.EventTrigger(EventConfig.P_Attack);
            EventManager.instance.EventTrigger<float>(EventConfig.E_BeAttacked, damageMultipier);
        }
        else if (beatType == BeatType.Defense)
        {
            EventManager.instance.EventTrigger(EventConfig.P_Defense);
        }
    }

    private void Update()
    {
        if (!IsBeatHittable())
            return;
        // if (!IsStreak)
        // {
        //     trackController.LaneMask.enabled = false;
        // }
        // else
        // {
        //     trackController.LaneMask.enabled = true;
        // }
    }

    public void OnStreakEnter()
    {
        Debug.Log("OnStreakEnter");
        OnHitted();
        trackController.hitterAnimator.SetTrigger("Hold");
        trackController.LaneMask.enabled = true;
        if (beatType == BeatType.Attack)
        {
            EventManager.instance.EventTrigger(EventConfig.P_Streak);
            EventManager.instance.EventTrigger<float>(EventConfig.E_BeAttacked, damageMultipier);
        }
        else if (beatType == BeatType.Defense)
        {
            EventManager.instance.EventTrigger(EventConfig.P_Defense);
        }
        CheckAccuracy();
    }
    public void OnStreakExit()
    {
        Debug.Log("OnStreakExit");
        trackController.LaneMask.enabled = false;
        trackController.hitterAnimator.SetTrigger("UnHold");
        if (IsStreak && beatType == BeatType.Attack)
        {
            EventManager.instance.EventTrigger(EventConfig.P_StopStreak);
        }
        // EventManager.instance.EventTrigger(EventConfig.P_StopStreak);
        // EventManager.instance.EventTrigger(EventConfig.E_StopStreak);
    }

    public void OnStreakInterrupt()
    {
        Debug.Log("OnStreakInterrupt");
        if (beatType == BeatType.Attack)
        {
            EventManager.instance.EventTrigger(EventConfig.P_StopStreak);
        }
        else if (beatType == BeatType.Defense)
        {
            EventManager.instance.EventTrigger<float>(EventConfig.P_BeAttacked, damageMultipier);
        }
    }

    public void OnStreakUpdate()
    {
        Debug.Log("OnStreakUpdate");
    }
    private void OnHitted()
    {
        visual.color = Color.Lerp(visual.color, Color.black, 0.3f);
    }

    public void OnMiss()
    {
        OnStreakExit();
        if (IsBeatEnd())
            return;
        UIManager.instance.GetPanel<UI_Accuracy>("UI_Accuracy").ShowAccuracy("Missing");
        if (beatType == BeatType.Defense)
        {
            EventManager.instance.EventTrigger<float>(EventConfig.P_BeAttacked, damageMultipier);
        }
    }

    void CheckAccuracy()
    {
        int beatTime = trackedEvent.StartSample;
        int curTime = rhythmController.DelayedSampleTime;
        int perfectWindow = rhythmController.PerfectWindowSampleWidth;
        if (Mathf.Abs(beatTime - curTime) <= perfectWindow)
        {
            UIManager.instance.GetPanel<UI_Accuracy>("UI_Accuracy").ShowAccuracy("Perfect");
            damageMultipier = 1.5f;
        }
        else
        {
            UIManager.instance.GetPanel<UI_Accuracy>("UI_Accuracy").ShowAccuracy("Good");
            damageMultipier = 1f;
        }
    }
    public void OnClear()
    {
        Destroy(this.gameObject);
    }

    #endregion
}