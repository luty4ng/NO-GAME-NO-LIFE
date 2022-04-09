using UnityEngine;
using System.Collections.Generic;
using SonicBloom.Koreo;
using GameKit;
using UnityEngine.UI;

public class TrackController : MonoBehaviour
{
    #region Fields
    public Protagonist protagonist;
    public Enemy enemy;
    public KeyCode keyboardButton;
    public List<string> matchedPayloads = new List<string>();
    public RectTransform spawn;
    public RectTransform hitter;
    public Beats BeatsArchetype;
    public Mask LaneMask;
    public Animator hitterAnimator;
    [SerializeField] List<KoreographyEvent> trackEvents = new List<KoreographyEvent>();
    [SerializeField] Queue<Beats> trackedBeats = new Queue<Beats>();
    RhythmController rhythmController;
    int pendingEventIdx = 0;
    public bool isPlayerSpreaking = false;

    #endregion

    #region Methods
    public void Initialize(RhythmController controller)
    {
        rhythmController = controller;
        LaneMask.enabled = false;
        hitterAnimator = hitter.GetComponent<Animator>();
    }
    void Update()
    {
        if (MusicBattleRegulator.current.IsDialoging)
            return;

        if (Timer.isPause)
            return;



        CheckSpawn();

        // if (!CheckSpreak())
        //     CheckHit();
        if (Input.GetKeyDown(keyboardButton))
        {
            hitterAnimator.SetTrigger("Hit");
            if (!CheckSpreak())
                CheckHit();
        }
        if (Input.GetKeyUp(keyboardButton))
            CheckSpreakLose();

        if (trackedBeats.Count > 0 && trackedBeats.Peek().IsBeatMissed() && !isPlayerSpreaking)
        {
            trackedBeats.Peek().OnMiss();
            if (!trackedBeats.Peek().IsStreak)
                trackedBeats.Dequeue();
        }
        KeepSpreaking();

        if (trackedBeats.Count > 0 && trackedBeats.Peek().IsBeatEnd() && trackedBeats.Peek().IsStreak)
            trackedBeats.Dequeue();
    }

    Beats GetFreshBeats()
    {
        Beats beat = GameObject.Instantiate<Beats>(BeatsArchetype);
        beat.gameObject.SetActive(true);
        beat.enabled = true;
        return beat;
    }
    void CheckHit()
    {
        if (trackedBeats.Count > 0 && trackedBeats.Peek().IsBeatEnter())
        {
            if (trackedBeats.Peek().IsAttack)
                MusicBattleRegulator.current.PlaySoundClip(MusicBattleRegulator.current.audioMono.ATTACK_SOUND);
            else
                MusicBattleRegulator.current.PlaySoundClip(MusicBattleRegulator.current.audioMono.DEFENSE_SOUND);
            
            trackedBeats.Peek().OnHit();
            if (!trackedBeats.Peek().IsStreak)
                trackedBeats.Dequeue();
        }
    }
    bool CheckSpreak()
    {
        if (trackedBeats.Count > 0 && trackedBeats.Peek().IsBeatSpreakable() && !isPlayerSpreaking)
        {
            if (trackedBeats.Peek().IsAttack)
                MusicBattleRegulator.current.PlaySoundClip(MusicBattleRegulator.current.audioMono.ATTACK_SOUND);
            else
                MusicBattleRegulator.current.PlaySoundClip(MusicBattleRegulator.current.audioMono.DEFENSE_SOUND);
            trackedBeats.Peek().OnPlayerStreakEnter();
            return true;
        }
        return false;
    }

    void KeepSpreaking()
    {
        if (trackedBeats.Count == 0)
            return;
        Beats beat = trackedBeats.Peek();

        // if (isPlayerSpreaking)
        //     beat.OnPlayerStreakUpdate();
    }

    void CheckSpreakLose()
    {
        if (trackedBeats.Count > 0 && isPlayerSpreaking)
        {
            Beats beats = trackedBeats.Peek();
            beats.OnPlayerStreakInterrupt();
        }
    }
    void CheckSpawn()
    {
        int offsetTime = (int)(rhythmController.beatTravelTime * rhythmController.SampleRate);
        int currentTime = rhythmController.DelayedSampleTime;
        while (pendingEventIdx < trackEvents.Count && trackEvents[pendingEventIdx].StartSample < currentTime + offsetTime)
        {
            KoreographyEvent evt = trackEvents[pendingEventIdx];
            Beats beat = GetFreshBeats();
            trackedBeats.Enqueue(beat);
            beat.Initialize(evt, this, rhythmController);
            if (!trackEvents[pendingEventIdx].IsOneOff())
            {
                float diff = trackEvents[pendingEventIdx].EndSample - trackEvents[pendingEventIdx].StartSample;
                beat.SetWidth(diff);
            }
            beat.SetInitialMovement();
            hitter.SetSiblingIndex(LaneMask.transform.childCount - 1);
            pendingEventIdx++;
        }
    }
    public void AddEvent(KoreographyEvent evt) => trackEvents.Add(evt);
    public bool DoesMatchPayload(string payload)
    {
        bool bMatched = false;
        for (int i = 0; i < matchedPayloads.Count; ++i)
        {
            if (payload == matchedPayloads[i])
            {
                bMatched = true;
                break;
            }
        }
        return bMatched;
    }

    #endregion
}
