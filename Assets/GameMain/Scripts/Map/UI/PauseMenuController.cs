using System;
using GameKit;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject panel;

    private AudioSource[] audioSources;

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }

    private void ToggleDialogActive()
    {
        bool active = !panel.activeSelf;
        audioSources[active ? 0 : 1].Play();
        Time.timeScale = active ? 0f : 1f;
        panel.SetActive(active);
        MapRegulator.current.ReportDialogSetActive(active);
        MapRegulator.current.gamePaused = active;
        EventManager.instance.EventTrigger<bool>(EventConfig.Game_Pase, active);
    }
    public void Correction()
    {
        Scheduler.instance.LoadSceneSwipe("Level Correction");
    }

    public void ContinueGame()
    {
        if (panel.activeSelf)
        {
            ToggleDialogActive();
        }
    }

    public void ExitToMain()
    {
        MapGlobals.SwitchToMain();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleDialogActive();
        }
    }
}