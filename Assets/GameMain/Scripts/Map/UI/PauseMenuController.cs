using System;
using GameKit;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject panel;

    private void SetDialogActive(bool active)
    {
        panel.SetActive(active);
        MapGlobals.ReportDialogSetActive(active);
        MapGlobals.GamePaused = active;
    }

    public void ContinueGame()
    {
        SetDialogActive(false);
    }

    public void ExitToMain()
    {
        Scheduler.instance.SwitchSceneSwipe("S_Menu_New");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetDialogActive(!panel.activeSelf);
        }
    }
}