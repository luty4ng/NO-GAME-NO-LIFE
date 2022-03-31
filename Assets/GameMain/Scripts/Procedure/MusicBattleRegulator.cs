using UnityEngine;
using GameKit;
public class MusicBattleRegulator : Regulator<MusicBattleRegulator>
{
    public string pausePanelName = "UI_Settings";
    public RhythmController rhythmController;
    public AudioSource globalSoundSource;
    public Protagonist protagonist;
    public string lastScene;
    public string finishScene;
    public int levelClear;
    private AudioSource globalMusicSource;
    public AudioMono audioMono;
    private bool battleEnd = false;
    public DialogSet dialogSet;
    public MusicDialogUIController dialogUIController;
    public bool IsDialoging
    {
        get
        {
            if (dialogSet != null)
                return dialogUIController.isActive;
            else
                return false;
        }

    }
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
        Timer.isPause = false;
        globalSoundSource = GetComponent<AudioSource>();
        globalMusicSource = rhythmController.audioCom;
        globalMusicSource.volume = 1;
        protagonist = GameObject.Find("Protagonist").GetComponent<Protagonist>();
    }

    public void Pause()
    {
        PlaySoundClip(audioMono.GAME_PAUSE);
        GetUI(pausePanelName).Show();
        Timer.isPause = true;
        EventManager.instance.EventTrigger<bool>(EventConfig.Game_Pase, true);
    }

    public void Continue()
    {
        PlaySoundClip(audioMono.GAME_CONT);
        GetUI(pausePanelName).Hide();
        Timer.isPause = false;
        EventManager.instance.EventTrigger<bool>(EventConfig.Game_Pase, false);
    }

    private void Update()
    {
        CheckPause();
        CheckEnding();

        if (Input.GetKeyDown(KeyCode.Tab))
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
        Debug.Log(UIManager.instance.GetPanel<UI_Ending>("UI_Ending"));
        bool isWin = (protagonist.Health > protagonist.enemy.Health);
        UIManager.instance.GetPanel<UI_Ending>("UI_Ending").isWin = isWin;
        UIManager.instance.GetPanel<UI_Ending>("UI_Ending").Show();
        battleEnd = true;



        StartCoroutine(DelayedExcute(() =>
        {
            UIManager.instance.GetPanel<UI_Ending>("UI_Ending").Hide();
            if (isWin)
            {
                dialogUIController.ShowDialog(start: false);
            }
            else
                Pause();
        }, 5f));

    }

    public void CheckEnding()
    {
        if (Timer.isPause)
            return;
        if (battleEnd)
            return;

        if (rhythmController.MuiscStartPlay && !rhythmController.audioCom.isPlaying && !Timer.isPause)
        {
            BattleEnding();
        }
    }

    public void ReturnToPrevious()
    {
        current.SwitchSceneSwipe(lastScene);
    }

    public void CompleteLevel()
    {
        MapGlobals.CurrentLevel = levelClear;
        current.SwitchSceneSwipe(finishScene);
    }

    public void CompleteLevelWithCG()
    {
        MapGlobals.CurrentLevel = levelClear;
        current.SwitchSceneSwipe(finishScene);
    }

    public void ReturnToMain()
    {
        MapGlobals.SwitchToMain();
    }

    public void PlaySoundClip(AudioClip clip)
    {
        if (globalSoundSource.isPlaying)
            globalSoundSource.Stop();
        globalSoundSource.clip = clip;
        globalSoundSource.Play();
    }
}