using System;
using GameKit;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    public GameObject continueButton;

    private void Start()
    {
        var hasSave = MapGlobals.HasSave;
        continueButton.GetComponent<Button>().interactable = hasSave;
    }

    public void NewGame()
    {
        MapGlobals.SwitchToStartSceneAndOverrideSave();
    }

    public void ContinueGame()
    {
        MapGlobals.SwitchToSceneUsingSave();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}