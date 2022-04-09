using UnityEngine;
using System.Collections.Generic;
using SonicBloom.Koreo;
using GameKit;
using UnityEngine.UI;

public class TrackC : MonoBehaviour
{
    public List<string> matchedPayloads = new List<string>();
    public RectTransform spawn;
    public RectTransform hitter;
    public BeatC BeatCArchetype;
    public Animator hitterAnimator;
    public ParserC parser;
    public Transform lane;
    private List<KoreographyEvent> events = new List<KoreographyEvent>();
    private Queue<BeatC> beats = new Queue<BeatC>();
    private int eventIndex = 0;

    #region Properties
    public BeatC currentBeat
    {
        get { return beats.Peek(); }
    }
    # endregion


    #region Methods
    public void Initialize(RhythmController controller)
    {
        hitterAnimator = hitter.GetComponent<Animator>();
    }
    void Update()
    {
        TrySpawnBeat();
        if (Input.GetKeyDown(KeyCode.A))
            TryHitBeat();
        CheckMiss();

        if (beats.Count == 0)
            return;
    }

    void TryHitBeat()
    {
        if (beats.Count > 0 && currentBeat.IsEnter())
        {
            // MusicBattleRegulator.current.PlaySoundClip(MusicBattleRegulator.current.audioMono.HIT_SOUND);
            currentBeat.OnHit();
            beats.Dequeue();
        }
    }

    void CheckMiss()
    {
        if (beats.Count > 0 && currentBeat.IsMiss())
        {
            currentBeat.OnMiss();
            beats.Dequeue();
        }
    }

    void TrySpawnBeat()
    {
        int offsetTime = (int)(parser.travelTime * parser.SampleRate);
        int currentTime = parser.CurrentSampleTime;
        while (eventIndex < events.Count && events[eventIndex].StartSample < currentTime + offsetTime)
        {
            KoreographyEvent evt = events[eventIndex];
            BeatC beat = GameObject.Instantiate<BeatC>(BeatCArchetype);
            beat.gameObject.SetActive(true);
            beat.enabled = true;
            beats.Enqueue(beat);
            beat.Initialize(evt, this, parser);
            beat.SetInitialMovement();
            eventIndex++;
        }
    }
    public void AddEvent(KoreographyEvent evt) => events.Add(evt);
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
