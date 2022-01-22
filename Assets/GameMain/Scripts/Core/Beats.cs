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
    CanvasGroup canvasGroup;
    float offsetWidth = 0;
    float damageMultipier = 1;
    bool isEnemyStreak = false;
    bool hasCheckMiss = false;

    public bool IsStreak
    {
        get
        {
            return !trackedEvent.IsOneOff();
        }
    }

    public bool IsAttack
    {
        get
        {
            return beatType == BeatType.Attack;
        }
    }

    #region Methods
    private void Start()
    {
        visual = GetComponent<Image>();
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        // Debug.Log(IsBeatSpreakable() && !IsAttack && IsStreak);
        if (IsBeatSpreakable() && !IsAttack && IsStreak && !isEnemyStreak)
        {
            isEnemyStreak = true;
            EventManager.instance.EventTrigger(EventConfig.E_Streak);
        }

        if (IsBeatHittable() && !IsAttack && !IsStreak)
            EventManager.instance.EventTrigger(EventConfig.E_Attack);
    }
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
        EventManager.instance.AddEventListener<bool>(EventConfig.Game_Pase, Pause);
        tweenSeq.Append(rectTransform.DOLocalMoveX(targetPosX, moveTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (IsStreak)
                OnPlayerStreakExit();
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

    public bool IsBeatHittable()
    {
        int beatTime = trackedEvent.StartSample;
        int curTime = rhythmController.DelayedSampleTime;
        int hitWindow = rhythmController.HitWindowSampleWidth;
        return (Mathf.Abs(beatTime - curTime) <= hitWindow);
    }

    public bool IsBeatSpreakable()
    {
        if (!IsStreak)
            return false;
        return IsBeatHittable();
    }

    public bool IsBeatMissed()
    {

        int startTime = trackedEvent.StartSample;
        int curTime = rhythmController.DelayedSampleTime;
        int hitWindow = rhythmController.HitWindowSampleWidth;
        bool isMiss = (curTime - startTime > hitWindow);
        if (!IsStreak)
        {
            return isMiss;
        }
        else
        {
            if (!hasCheckMiss && isMiss)
            {
                hasCheckMiss = true;
                return true;
            }
            return false;
        }

    }

    public bool IsBeatEnd()
    {
        int endTime = trackedEvent.EndSample;
        int curTime = rhythmController.DelayedSampleTime;
        int hitWindow = rhythmController.HitWindowSampleWidth;
        return (Mathf.Abs(endTime - curTime) <= hitWindow);
    }

    public void OnHit()
    {
        OnVisualUpdate();
        animator.SetTrigger("Collapse");
        // trackController.isPlayerSpreaking = false;
        trackController.hitterAnimator.SetTrigger("Hit");
        // trackController.LaneMask.enabled = false;
        if (!IsStreak && IsAttack)
        {
            EventManager.instance.EventTrigger(EventConfig.P_Attack);
            EventManager.instance.EventTrigger<float, bool>(EventConfig.E_BeAttacked, damageMultipier, IsStreak);
        }
        else if (!IsStreak && !IsAttack)
        {
            EventManager.instance.EventTrigger(EventConfig.P_Defense);
        }
        CheckAccuracy();
    }

    public void OnPlayerStreakEnter()
    {
        Debug.Log("OnPlayerStreakEnter");
        OnVisualUpdate();
        trackController.isPlayerSpreaking = true;
        trackController.LaneMask.enabled = true;
        // trackController.hitterAnimator.SetTrigger("Hold");
        trackController.hitterAnimator.SetBool("Streaking", true);

        if (IsStreak && IsAttack)
        {
            EventManager.instance.EventTrigger(EventConfig.P_Streak);
            EventManager.instance.EventTrigger<float, bool>(EventConfig.E_BeAttacked, damageMultipier, IsStreak);
        }
        else if (IsStreak && !IsAttack)
        {
            EventManager.instance.EventTrigger(EventConfig.P_StreakDefense);
        }
        CheckAccuracy();
    }
    public void OnPlayerStreakExit()
    {
        Debug.Log("OnPlayerStreakExit");
        if (IsStreak && IsAttack && trackController.isPlayerSpreaking)
        {
            EventManager.instance.EventTrigger(EventConfig.P_StopStreak);
            EventManager.instance.EventTrigger(EventConfig.E_StopLoop); 
        }
        else if (IsStreak && !IsAttack && trackController.isPlayerSpreaking)
        {
            EventManager.instance.EventTrigger(EventConfig.E_StopStreak);
            EventManager.instance.EventTrigger(EventConfig.P_StopStreakDefense); 
        }
        EventManager.instance.EventTrigger(EventConfig.P_StopLoop);
        trackController.hitterAnimator.SetBool("Streaking", false);
        trackController.isPlayerSpreaking = false;
        trackController.LaneMask.enabled = false;
        isEnemyStreak = false;
        canvasGroup.alpha = 0;
    }

    public void OnPlayerStreakInterrupt()
    {
        Debug.Log("OnPlayerStreakInterrupt");
        if (IsStreak && IsAttack && trackController.isPlayerSpreaking)
        {
            EventManager.instance.EventTrigger(EventConfig.P_StopStreak);
            EventManager.instance.EventTrigger(EventConfig.E_StopLoop);
        }
        else if (IsStreak && !IsAttack && trackController.isPlayerSpreaking)
        {
            EventManager.instance.EventTrigger(EventConfig.P_StopStreakDefense);
        }
        EventManager.instance.EventTrigger(EventConfig.P_StopLoop);  
        trackController.hitterAnimator.SetBool("Streaking", false);
        trackController.isPlayerSpreaking = false;
    }

    public void OnPlayerStreakUpdate()
    {
        Debug.Log("OnPlayerStreakUpdate");
    }
    private void OnVisualUpdate()
    {
        visual.color = Color.Lerp(visual.color, Color.black, 0.3f);
    }

    public void OnMiss()
    {
        Debug.Log("IsMissing");
        UIManager.instance.GetPanel<UI_Accuracy>("UI_Accuracy").ShowAccuracy("Missing");
        if (!IsAttack)
        {
            EventManager.instance.EventTrigger<float, bool>(EventConfig.P_BeAttacked, damageMultipier, IsStreak);
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