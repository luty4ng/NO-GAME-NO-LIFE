using System;
using System.Collections.Generic;
using GameKit;
using UnityEngine;
using UnityEngine.UI;
public class MusicDialogUIController : MonoBehaviour
{
    public GameObject overlay;
    public GameObject window;
    public Text UiText;
    public Animator windowAnimator;
    public AudioClip dialogFlip;
    public AudioClip dialogOpen;
    public AudioClip dialogClose;
    public AudioSource dialogSource;

    public bool isActive = false;
    private bool isStart = true;
    private void Start()
    {
        ShowDialog(true);
    }
    public void ShowDialog(bool start = true)
    {
        if (start && !MusicBattleRegulator.current.dialogSet.hasStartDialog)
            return;
        if (!start && !MusicBattleRegulator.current.dialogSet.hasEndDialog)
        {
            MusicBattleRegulator.current.CompleteLevel();
            return;
        }

        windowAnimator.SetTrigger("Show");
        dialogSource.clip = dialogOpen;
        dialogSource.Play();
        overlay.SetActive(true);
        UiText.gameObject.SetActive(true);
        isActive = true;
        isStart = start;
        if (isStart)
            UiText.text = MusicBattleRegulator.current.dialogSet.StartFirstPhase();
        else
            UiText.text = MusicBattleRegulator.current.dialogSet.EndFirstPhase();
    }

    public void HideDialog()
    {
        overlay.SetActive(false);
        UiText.gameObject.SetActive(false);
        windowAnimator.SetTrigger("Hide");
        dialogSource.clip = dialogClose;
        dialogSource.Play();
        isActive = false;
    }

    private void Update()
    {
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                string dialog = "";
                if (isStart)
                    dialog = MusicBattleRegulator.current.dialogSet.StartNextPhase();
                else
                    dialog = MusicBattleRegulator.current.dialogSet.EndNextPhase();

                dialogSource.clip = dialogFlip;
                dialogSource.Play();
                if (dialog != "last")
                    UiText.text = dialog;
                else
                {
                    HideDialog();
                    if (!isStart)
                    {
                        if (ScenesManager.instance.GetSceneAt(1).name == "Level 4")
                            EventManager.instance.EventTrigger(EventConfig.FINAL_CG);
                        else
                            MusicBattleRegulator.current.CompleteLevel();
                    }
                }
            }
        }
    }
}