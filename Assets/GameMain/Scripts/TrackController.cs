using UnityEngine;
using System.Collections.Generic;
using SonicBloom.Koreo;

public class TrackController : MonoBehaviour
{
    #region Fields
    public KeyCode keyboardButton;
    public List<string> matchedPayloads = new List<string>();
    public RectTransform spawn;
    public RectTransform hitter;
    public Beats BeatsArchetype;
    List<KoreographyEvent> trackEvents = new List<KoreographyEvent>();
    Queue<Beats> trackedBeats = new Queue<Beats>();
    RhythmController rhythmController;
    int pendingEventIdx = 0;
    public bool isSpreaking = false;
    #endregion

    #region Methods
    public void Initialize(RhythmController controller) => rhythmController = controller;
    public void Restart(int newSampleTime = 0)
    {
        for (int i = 0; i < trackEvents.Count; ++i)
        {
            if (trackEvents[i].StartSample >= newSampleTime)
            {
                pendingEventIdx = i;
                break;
            }
        }
        int numToClear = trackedBeats.Count;
        for (int i = 0; i < numToClear; ++i)
        {
            trackedBeats.Dequeue().OnClear();
        }
    }

    void Update()
    {
        while (trackedBeats.Count > 0 && trackedBeats.Peek().IsBeatMissed())
        {
            Beats beats = trackedBeats.Dequeue();
            beats.OnMiss();
        }
        CheckSpawn();

        CheckSpreak();
        CheckHit();

        if (Input.GetKeyDown(keyboardButton))
        {
            if (!CheckSpreak())
                CheckHit();
        }
        if (Input.GetKeyUp(keyboardButton))
            CheckSpreakLose();

        KeepSpreaking();
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
        if (trackedBeats.Count > 0 && trackedBeats.Peek().IsBeatHittable())
        {
            Beats beat = trackedBeats.Dequeue();
            beat.OnHit();
        }
    }

    bool CheckSpreak()
    {
        // if (trackedBeats.Count > 0)
        // {
        //     Debug.Log(trackedBeats.Peek().IsBeatSpreakable());
        // }
        if (trackedBeats.Count > 0 && trackedBeats.Peek().IsBeatSpreakable() && isSpreaking == false)
        {
            isSpreaking = true;
            trackedBeats.Peek().OnStreakEnter();
            return true;
        }
        return false;
    }

    void KeepSpreaking()
    {
        if (trackedBeats.Count > 0 && isSpreaking)
        {
            Beats beats = trackedBeats.Peek();
            beats.OnStreakUpdate();
        }
    }

    void CheckSpreakLose()
    {
        if (trackedBeats.Count > 0 && isSpreaking)
        {
            Beats beats = trackedBeats.Dequeue();
            isSpreaking = false;
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
            pendingEventIdx++;
        }
    }

    public void AddEvent(KoreographyEvent evt)
    {
        trackEvents.Add(evt);
    }
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
