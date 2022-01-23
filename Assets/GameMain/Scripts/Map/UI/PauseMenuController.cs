using System;
using GameKit;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject panel;

    private AudioSource[] _audioSources;

    private void Start()
    {
        _audioSources = GetComponents<AudioSource>();
    }

    private void SetDialogActive(bool active)
    {
        _audioSources[active ? 0 : 1].Play();
        panel.SetActive(active);
        MapRegulator.current.ReportDialogSetActive(active);
        MapGlobals.GamePaused = active;
    }

    public void ContinueGame()
    {
        SetDialogActive(false);
    }

    public void ExitToMain()
    {
        MapGlobals.SwitchToMain();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetDialogActive(!panel.activeSelf);
        }
    }
}