using UnityEngine;
using System.Collections.Generic;
using SonicBloom.Koreo;

public class TrackController : MonoBehaviour
{
    #region Fields
    public KeyCode keyboardButton;

    [Tooltip("A list of Payload strings that Koreography Events will contain for this Lane.")]
    public List<string> matchedPayloads = new List<string>();
    public RectTransform spawn;
    public RectTransform hitter;
    List<KoreographyEvent> trackEvents = new List<KoreographyEvent>();
    Queue<Beats> trackedBeats = new Queue<Beats>();
    RhythmController rhythmController;
    int pendingEventIdx = 0;


    #endregion

    #region Methods
    public void Initialize(RhythmController controller) => rhythmController = controller;
    // This method controls cleanup, resetting the internals to a fresh state.
    public void Restart(int newSampleTime = 0)
    {
        // Find the index of the first event at or beyond the target sample time.
        for (int i = 0; i < trackEvents.Count; ++i)
        {
            if (trackEvents[i].StartSample >= newSampleTime)
            {
                pendingEventIdx = i;
                break;
            }
        }

        // Clear out the tracked notes.
        int numToClear = trackedBeats.Count;
        for (int i = 0; i < numToClear; ++i)
        {
            trackedBeats.Dequeue().OnClear();
        }
    }

    void Start()
    {

    }

    void Update()
    {
        // Clear out invalid entries.
        while (trackedBeats.Count > 0 && trackedBeats.Peek().IsBeatMissed())
            trackedBeats.Dequeue();
        CheckSpawn();
        if (Input.GetKeyDown(keyboardButton))
            CheckHit();
    }

    public void CheckHit()
    {
        if (trackedBeats.Count > 0 && trackedBeats.Peek().IsBeatHittable())
        {
            Beats beat = trackedBeats.Dequeue();
            beat.OnHit();
        }
    }
    void CheckSpawn()
    {
        int offsetTime = (int)(rhythmController.beatTravelTime * rhythmController.SampleRate);
        int currentTime = rhythmController.DelayedSampleTime;
        while (pendingEventIdx < trackEvents.Count && trackEvents[pendingEventIdx].StartSample < currentTime + offsetTime)
        {

            KoreographyEvent evt = trackEvents[pendingEventIdx];
            Beats beat = rhythmController.GetFreshBeats();
            beat.Initialize(evt, this, rhythmController);
            trackedBeats.Enqueue(beat);
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
