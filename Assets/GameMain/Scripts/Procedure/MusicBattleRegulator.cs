using UnityEngine;
using GameKit;
public class MusicBattleRegulator : Regulator<MusicBattleRegulator>
{
    public string pausePanelName = "UI_Settings";
    public RhythmController rhythmController;
    public AudioSource globalSoundSource;
    public Protagonist protagonist;
    public string finishScene = "S_Select";
    private AudioSource globalMusicSource;
    public AudioMono audioMono;
    private bool battleEnd = false;
    private void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GetUI(pausePanelName).isActive)
                Pause();
            else
                Continue();
        }
    }

    private void Start()
    {
        globalSoundSource = GetComponent<AudioSource>();
        globalMusicSource = rhythmController.audioCom;
        globalMusicSource.volume = 1;
        protagonist = GameObject.Find("Protagonist").GetComponent<Protagonist>();
    }

    public void Pause()
    {
        MusicBattleRegulator.current.PlaySoundClip(MusicBattleRegulator.current.audioMono.GAME_PAUSE);
        GetUI(pausePanelName).Show();
        Timer.isPause = true;
        EventManager.instance.EventTrigger<bool>(EventConfig.Game_Pase, true);
    }

    public void Continue()
    {
        MusicBattleRegulator.current.PlaySoundClip(MusicBattleRegulator.current.audioMono.GAME_CONT);
        GetUI(pausePanelName).Hide();
        Timer.isPause = false;
        EventManager.instance.EventTrigger<bool>(EventConfig.Game_Pase, false);
    }

    private void Update()
    {
        CheckPause();
        BattleEnding();
    }

    public void ChangeMusicVolume(float v)
    {
        if (globalMusicSource == null)
            return;
        globalMusicSource.volume = v;
    }

    public void ChangeSoundVolume(float v)
    {
        if (globalSoundSource == null)
            return;
        globalSoundSource.volume = v;
    }

    public void ChangeMasterVolume(float v)
    {
        ChangeSoundVolume(v);
        ChangeMusicVolume(v);
    }

    public void BattleEnding()
    {
        if (Timer.isPause)
            return;
        if (battleEnd)
            return;
        if (rhythmController.MuiscStartPlay && !rhythmController.audioCom.isPlaying)
        {
            Debug.Log(UIManager.instance.GetPanel<UI_Ending>("UI_Ending"));
            bool isWin = (protagonist.Health > protagonist.enemy.Health);
            UIManager.instance.GetPanel<UI_Ending>("UI_Ending").isWin = isWin;
            UIManager.instance.GetPanel<UI_Ending>("UI_Ending").Show();
            battleEnd = true;
            StartCoroutine(DelayedExcute(() =>
            {
                UIManager.instance.GetPanel<UI_Ending>("UI_Ending").Hide();
                // Pause();
                if (isWin)
                    MusicBattleRegulator.current.SwitchSceneSwipe(finishScene);
                else
                    Pause();
            }, 5f));
        }
    }

    public void PlaySoundClip(AudioClip clip)
    {
        if (globalSoundSource.isPlaying)
            globalSoundSource.Stop();
        globalSoundSource.clip = clip;
        globalSoundSource.Play();
    }
}