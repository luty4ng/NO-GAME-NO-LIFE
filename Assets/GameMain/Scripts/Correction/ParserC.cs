using UnityEngine;
using System.Collections.Generic;
using SonicBloom.Koreo;
using GameKit;

public class ParserC : MonoBehaviour
{
    #region Fields

    [EventID]
    public string eventID;
    public float travelTime = 2f;
    public float leadInTime = 4f;
    public List<TrackC> tracks = new List<TrackC>();
    public AudioSource audioSource;
    public UI_Accuracy_Mono uI_Accuracy;
    private float leadInTimeLeft;
    private float leadinTimeLeft;
    private Koreography koreo;
    private int hitWindowSize_Sample;
    private int perfectWindowSize_Sample;

    #endregion

    #region Properties
    public int HitWindowSampleWidth
    {
        get { return hitWindowSize_Sample; }
    }

    public int PerfectWindowSampleWidth
    {
        get { return perfectWindowSize_Sample; }
    }

    public int SampleRate
    {
        get { return koreo.SampleRate; }
    }

    public int CurrentSampleTime
    {
        get { return koreo.GetLatestSampleTime() - (int)(audioSource.pitch * leadinTimeLeft * SampleRate); }
    }
    #endregion

    #region Methods
    void Start()
    {
        koreo = Koreographer.Instance.GetKoreographyAtIndex(0);
        KoreographyTrackBase rhythmTrack = koreo.GetTrackByID(eventID);
        List<KoreographyEvent> rawEvents = rhythmTrack.GetAllEvents();

        // 初始化音乐节点信息
        for (int i = 0; i < rawEvents.Count; ++i)
        {
            KoreographyEvent evt = rawEvents[i];
            string payload = evt.GetTextValue();
            for (int j = 0; j < tracks.Count; ++j)
            {
                TrackC track = tracks[j];
                if (track.DoesMatchPayload(payload))
                {
                    track.AddEvent(evt);
                    break;
                }
            }
        }

        // 初始化音乐延迟载入
        LeadInInit();
    }

    void LeadInInit()
    {
        if (leadInTime > 0f)
        {
            // 实际延迟时间 = 预设延迟时间 - Koregrapher预设延迟时间 
            leadinTimeLeft = leadInTime - Koreographer.Instance.EventDelayInSeconds;
        }
        else
        {
            leadInTime = 0;
            audioSource.Play();
        }
    }

    void Update()
    {        
        UpdateHitRange();
        CheckAudioPlay();
    }

    void CheckAudioPlay()
    {
        if (leadinTimeLeft > 0f)
        {
            leadinTimeLeft -= Time.deltaTime;
            if (leadinTimeLeft <= 0f)
            {
                audioSource.time = -leadinTimeLeft;
                audioSource.Play();
                leadinTimeLeft = 0f;
            }
        }
    }
    void UpdateHitRange()
    {
        hitWindowSize_Sample = (int)(0.001f * Rhythm.rhythmOffset * SampleRate);
        perfectWindowSize_Sample = (int)(0.001f * (Rhythm.rhythmOffset / 2) * SampleRate);
    }

    #endregion
}

