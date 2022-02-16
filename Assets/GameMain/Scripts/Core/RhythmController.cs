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
    [EventID]
    public string eventID;
    [Range(8f, 150f)]
    public float hitWindowRangeInMS = 80;
    public float beatTravelTime = 2.5f;
    public List<TrackController> tracks = new List<TrackController>();
    public float leadInTime;
    public AudioSource audioCom;
    public float leadInTimeLeft;
    float timeLeftToPlay;
    Koreography playingKoreo;
    int hitWindowRangeInSamples;
    int perfectWindowRangeInSamples;


    #endregion
    #region Properties
    public int HitWindowSampleWidth
    {
        get
        {
            return hitWindowRangeInSamples;
        }
    }

    public int PerfectWindowSampleWidth
    {
        get
        {
            return perfectWindowRangeInSamples;
        }
    }

    public float WindowSizeInUnits
    {
        get
        {
            return (1830 / beatTravelTime) * (hitWindowRangeInMS * 0.001f);
        }
    }

    public int SampleRate
    {
        get
        {
            return playingKoreo.SampleRate;
        }
    }

    public double TimePerSnap
    {
        get
        {
            return 0.5f;
        }
    }

    public double SamplesPerBeat
    {
        get
        {
            return playingKoreo.GetSamplesPerBeat(1);
        }
    }

    public int DelayedSampleTime
    {
        get
        {
            return playingKoreo.GetLatestSampleTime() - (int)(audioCom.pitch * leadInTimeLeft * SampleRate);
        }
    }

    public bool MuiscStartPlay
    {
        get
        {
            return leadInTimeLeft == 0;
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

        EventManager.instance.AddEventListener<bool>(EventConfig.Game_Pase, PauseAudio);
    }

    void PauseAudio(bool isPause)
    {
        if (isPause)
            audioCom.Pause();
        else
        {
            if (timeLeftToPlay > 0f)
            {
                CheckAudioPlay();
            }
            else
                audioCom.Play();
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
        if (Timer.isPause)
            return;
        
        if (MusicBattleRegulator.current.IsDialoging)
            return;
        UpdateInternalValues();
        if (leadInTimeLeft > 0f)
        {
            leadInTimeLeft = Mathf.Max(leadInTimeLeft - Time.deltaTime, 0f);
        }
        CheckAudioPlay();
    }

    void CheckAudioPlay()
    {
        if (timeLeftToPlay > 0f)
        {
            timeLeftToPlay -= Time.deltaTime;
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
        perfectWindowRangeInSamples = (int)(0.001f * (hitWindowRangeInMS / 2) * SampleRate);
    }


    public void Restart()
    {
        audioCom.Stop();
        audioCom.time = 0f;
        Koreographer.Instance.FlushDelayQueue(playingKoreo);
        playingKoreo.ResetTimings();
        InitializeLeadIn();
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveEventListener<bool>(EventConfig.Game_Pase, PauseAudio);
    }
    #endregion
}

