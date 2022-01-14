//----------------------------------------------
//            	   Koreographer                 
//    Copyright © 2014-2020 Sonic Bloom, LLC    
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using SonicBloom.Koreo;
using GameKit;

public class RhythmController : MonoBehaviour
{
    #region Fields

    [Tooltip("The Event ID of the track to use for target generation.")]
    [EventID]
    public string eventID;

    [Tooltip("The number of milliseconds (both early and late) within which input will be detected as a Hit.")]
    [Range(8f, 150f)]
    public float hitWindowRangeInMS = 80;

    [Tooltip("The number of units traversed per second by Note Objects.")]
    public float beatTravelTime = 2.5f;

    [Tooltip("The archetype (blueprints) to use for generating notes.  Can be a prefab.")]
    public Beats BeatsArchetype;

    [Tooltip("The list of Lane Controller objects that represent a lane for an event to travel down.")]
    public List<TrackController> tracks = new List<TrackController>();

    [Tooltip("The amount of time in seconds to provide before playback of the audio begins.  Changes to this value are not immediately handled during the lead-in phase while playing in the Editor.")]
    public float leadInTime;

    [Tooltip("The Audio Source through which the Koreographed audio will be played.  Be sure to disable 'Auto Play On Awake' in the Music Player.")]
    public AudioSource audioCom;

    // The amount of leadInTime left before the audio is audible.
    public float leadInTimeLeft;

    // The amount of time left before we should play the audio (handles Event Delay).
    float timeLeftToPlay;

    // Local cache of the Koreography loaded into the Koreographer component.
    Koreography playingKoreo;

    // Koreographer works in samples.  Convert the user-facing values into sample-time.  This will simplify
    //  calculations throughout.
    int hitWindowRangeInSamples;    // The sample range within which a viable event may be hit.

    // The pool for containing note objects to reduce unnecessary Instantiation/Destruction.
    Stack<Beats> BeatsPool = new Stack<Beats>();

    #endregion
    #region Properties
    public int HitWindowSampleWidth
    {
        get
        {
            return hitWindowRangeInSamples;
        }
    }

    // public float WindowSizeInUnits
    // {
    //     get
    //     {
    //         return beatSpeed * (hitWindowRangeInMS * 0.001f);
    //     }
    // }
    public int SampleRate
    {
        get
        {
            return playingKoreo.SampleRate;
        }
    }

    public int DelayedSampleTime
    {
        get
        {
            return playingKoreo.GetLatestSampleTime() - (int)(audioCom.pitch * leadInTimeLeft * SampleRate);
        }
    }

    #endregion
    #region Methods

    void Start()
    {
        InitializeLeadIn();
        for (int i = 0; i < tracks.Count; ++i)
            tracks[i].Initialize(this);

        playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(0);
        KoreographyTrackBase rhythmTrack = playingKoreo.GetTrackByID(eventID);
        List<KoreographyEvent> rawEvents = rhythmTrack.GetAllEvents();

        for (int i = 0; i < rawEvents.Count; ++i)
        {
            KoreographyEvent evt = rawEvents[i];
            string payload = evt.GetTextValue();
            for (int j = 0; j < tracks.Count; ++j)
            {
                TrackController track = tracks[j];
                if (track.DoesMatchPayload(payload))
                {
                    track.AddEvent(evt);
                    break;
                }
            }
        }
    }
    void InitializeLeadIn()
    {
        if (leadInTime > 0f)
        {
            leadInTimeLeft = leadInTime;
            timeLeftToPlay = leadInTime - Koreographer.Instance.EventDelayInSeconds;
        }
        else
        {
            audioCom.time = -leadInTime;
            audioCom.Play();
        }
        
    }

    void Update()
    {
        UpdateInternalValues();
        if (leadInTimeLeft > 0f)
        {
            leadInTimeLeft = Mathf.Max(leadInTimeLeft - Time.unscaledDeltaTime, 0f);
        }

        if (timeLeftToPlay > 0f)
        {
            timeLeftToPlay -= Time.unscaledDeltaTime;
            if (timeLeftToPlay <= 0f)
            {
                audioCom.time = -timeLeftToPlay;
                audioCom.Play();
                timeLeftToPlay = 0f;
            }
        }
    }
    void UpdateInternalValues()
    {
        hitWindowRangeInSamples = (int)(0.001f * hitWindowRangeInMS * SampleRate);
    }

    public Beats GetFreshBeats()
    {
        Beats beat;
        if (BeatsPool.Count > 0)
        {
            beat = BeatsPool.Pop();
        }
        else
        {
            beat = GameObject.Instantiate<Beats>(BeatsArchetype);
        }

        beat.gameObject.SetActive(true);
        beat.enabled = true;

        return beat;
    }
    public void ReturnBeatsToPool(Beats obj)
    {
        if (obj != null)
        {
            obj.enabled = false;
            obj.gameObject.SetActive(false);
            BeatsPool.Push(obj);
        }
    }
    public void Restart()
    {
        audioCom.Stop();
        audioCom.time = 0f;
        Koreographer.Instance.FlushDelayQueue(playingKoreo);
        playingKoreo.ResetTimings();
        for (int i = 0; i < tracks.Count; ++i)
        {
            tracks[i].Restart();
        }
        InitializeLeadIn();
    }

    #endregion
}

