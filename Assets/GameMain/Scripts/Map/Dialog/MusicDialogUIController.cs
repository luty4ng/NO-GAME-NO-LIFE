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
    private void Start()
    {
        ShowDialog();
    }
    private void ShowDialog()
    {
        windowAnimator.SetTrigger("Show");
        dialogSource.clip = dialogOpen;
        dialogSource.Play();
        overlay.SetActive(true);
        UiText.gameObject.SetActive(true);
        isActive = true;
        UiText.text = MusicBattleRegulator.current.dialogSet.FirstPhase();
    }

    private void HideDialog()
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
                string dialog = MusicBattleRegulator.current.dialogSet.NextPhase();
                dialogSource.clip = dialogFlip;
                dialogSource.Play();
                if (dialog != "last")
                    UiText.text = dialog;
                else
                {
                    HideDialog();
                }
            }
        }
    }
}