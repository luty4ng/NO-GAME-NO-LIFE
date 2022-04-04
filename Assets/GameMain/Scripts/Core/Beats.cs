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
    Protagonist protagonist;
    Enemy enemy;
    float offsetWidth = 0;
    float damageMultipier = 1;
    public bool isEnemyStreak = false;
    bool missed = false;
    bool passed = false;
    bool entered = false;
    bool hasHitAttack = false;

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
        if (IsBeatEnter() && !IsAttack && IsStreak && !isEnemyStreak)
        {
            isEnemyStreak = true;
            enemy.Streak();
            // EventManager.instance.EventTrigger(EventConfig.E_Streak);
        }

        if (IsBeatCenter() && !IsAttack && !IsStreak && !hasHitAttack && !isEnemyStreak)
        {

            enemy.Attack();
            // EventManager.instance.EventTrigger(EventConfig.E_Attack);
            hasHitAttack = true;
        }
    }
    public void Initialize(KoreographyEvent evt, TrackController trackCtrl, RhythmController rhythmCtrl)
    {
        trackedEvent = evt;
        trackController = trackCtrl;
        rhythmController = rhythmCtrl;

        rectTransform = this.GetComponent<RectTransform>();
        rectTransform.SetParent(trackController.LaneMask.transform);
        rectTransform.anchoredPosition = trackCtrl.spawn.anchoredPosition;

        protagonist = trackController.protagonist;
        enemy = trackController.enemy;
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

    public bool IsBeatEnter()
    {
        int startTime = trackedEvent.StartSample;
        int curTime = rhythmController.DelayedSampleTime;
        int hitWindow = rhythmController.HitWindowSampleWidth;
        return (Mathf.Abs(startTime - curTime) <= hitWindow);
    }

    public bool IsBeatEnd()
    {
        int endTime = trackedEvent.EndSample;
        int curTime = rhythmController.DelayedSampleTime;
        int hitWindow = rhythmController.HitWindowSampleWidth;
        return (Mathf.Abs(endTime - curTime) <= hitWindow / 4);
    }

    public bool IsBeatCenter()
    {
        int startTime = trackedEvent.StartSample;
        int curTime = rhythmController.DelayedSampleTime;
        int hitWindow = rhythmController.HitWindowSampleWidth;
        return (Mathf.Abs(startTime - curTime) <= hitWindow / 4);
    }

    public bool IsBeatSpreakable()
    {
        if (!IsStreak)
            return false;
        return IsBeatEnter();
    }

    public bool IsBeatMissed()
    {

        int startTime = trackedEvent.StartSample;
        int curTime = rhythmController.DelayedSampleTime;
        int hitWindow = rhythmController.HitWindowSampleWidth;
        bool isMiss = (curTime - startTime > hitWindow);
        if (!IsStreak)
        {
            if (isMiss)
                hasHitAttack = false;
            return isMiss;
        }
        else
        {
            if (!missed && isMiss)
            {
                missed = true;
                return true;
            }
            return false;
        }

    }



    public bool IsBeatPass()
    {
        int endTime = trackedEvent.EndSample;
        int curTime = rhythmController.DelayedSampleTime;
        return curTime >= endTime;
    }

    public void OnHit()
    {
        OnVisualUpdate();
        animator.SetTrigger("Collapse");
        hasHitAttack = false;
        trackController.hitterAnimator.SetTrigger("Hit");
        // trackController.LaneMask.enabled = false;
        if (!IsStreak && IsAttack)
        {
            protagonist.Attack();
            enemy.BeAttack(damageMultipier);
            // EventManager.instance.EventTrigger(EventConfig.P_Attack);
            // EventManager.instance.EventTrigger<float, bool>(EventConfig.E_BeAttacked, damageMultipier, IsStreak);
        }
        else if (!IsStreak && !IsAttack)
        {
            protagonist.Defense();
            // EventManager.instance.EventTrigger(EventConfig.P_Defense);
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
            protagonist.Streak();
            enemy.BeStreak(damageMultipier);

            // EventManager.instance.EventTrigger(EventConfig.P_Streak);
            // EventManager.instance.EventTrigger<float, bool>(EventConfig.E_BeAttacked, damageMultipier, IsStreak);
        }
        else if (IsStreak && !IsAttack)
        {
            protagonist.DefenseStreak();
            // EventManager.instance.EventTrigger(EventConfig.P_StreakDefense);
        }
        CheckAccuracy();
    }
    public void OnPlayerStreakExit()
    {
        Debug.Log("OnPlayerStreakExit");
        if (IsAttack && trackController.isPlayerSpreaking)
        {
            protagonist.StopStreak();
            enemy.StopBeStreak();
            // EventManager.instance.EventTrigger(EventConfig.P_StopStreak);
            // EventManager.instance.EventTrigger(EventConfig.E_StopLoop);
        }
        else if (!IsAttack && trackController.isPlayerSpreaking)
        {
            enemy.StopStreak();
            protagonist.StopDefenseStreak();
            // EventManager.instance.EventTrigger(EventConfig.E_StopStreak);
            // EventManager.instance.EventTrigger(EventConfig.P_StopStreakDefense);
        }
        protagonist.StopBeStreak();

        if (isEnemyStreak)
        {
            enemy.StopStreak();
            isEnemyStreak = false;
        }

        // EventManager.instance.EventTrigger(EventConfig.P_StopLoop);
        trackController.hitterAnimator.SetBool("Streaking", false);
        trackController.isPlayerSpreaking = false;
        trackController.LaneMask.enabled = false;
        canvasGroup.alpha = 0;
    }

    public void OnPlayerStreakInterrupt()
    {
        Debug.Log("OnPlayerStreakInterrupt");
        if (IsStreak && IsAttack && trackController.isPlayerSpreaking)
        {
            protagonist.StopStreak();
            enemy.StopBeStreak();
            // EventManager.instance.EventTrigger(EventConfig.P_StopStreak);
            // EventManager.instance.EventTrigger(EventConfig.E_StopLoop);
        }
        else if (IsStreak && !IsAttack && trackController.isPlayerSpreaking)
        {
            protagonist.StopDefenseStreak();
            // EventManager.instance.EventTrigger(EventConfig.P_StopStreakDefense);
        }
        protagonist.StopBeStreak();
        // EventManager.instance.EventTrigger(EventConfig.P_StopLoop);
        trackController.hitterAnimator.SetBool("Streaking", false);
        trackController.isPlayerSpreaking = false;
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
            if (IsStreak)
                protagonist.BeStreak(damageMultipier);
            else
                protagonist.BeAttack(damageMultipier);
            // EventManager.instance.EventTrigger<float, bool>(EventConfig.P_BeAttacked, damageMultipier, IsStreak);
        }
    }

    public void MissHit()
    {
        Debug.Log("Miss Hit");
        UIManager.instance.GetPanel<UI_Accuracy>("UI_Accuracy").ShowAccuracy("Missing");
    }

    public void MissStreak()
    {
        Debug.Log("Miss Streak");
        UIManager.instance.GetPanel<UI_Accuracy>("UI_Accuracy").ShowAccuracy("Missing");
        protagonist.BeStreak(damageMultipier);
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