using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKit;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public sealed class UI_Settings : UIGroup
{
    private Slider volumeSlider;
    private GameObject currentBtns;
    public GameObject rootBtns;
    public GameObject settingBtns;
    public GameObject keyBindBtns;
    public UI_Rebind keybind;

    protected override void OnStart()
    {
        base.OnStart();
        volumeSlider = GetUIComponent<Slider>("Sld_Volume");
        currentBtns = rootBtns;
    }
    protected override FindType findType { get { return FindType.All; } }
    public override void Show(UnityAction callback = null)
    {
        ResetBtns();
        canvasGroup.alpha = 1;
        this.gameObject.SetActive(true);
        callback?.Invoke();
    }

    public override void Hide(UnityAction callback = null)
    {
        foreach (var uiComp in this.GetUIComponents())
        {
            uiComp.transform.localScale = Vector3.one;
        }
        canvasGroup.alpha = 0;
        this.gameObject.SetActive(false);
        callback?.Invoke();
    }

    public void OnSliderValueChange()
    {
        MusicBattleRegulator.current.ChangeMasterVolume(volumeSlider.value);
    }

    public void Correction()
    {
        Debug.Log("load correction");
        Scheduler.instance.LoadSceneSwipe("Level Correction");
    }

    public void GoKeyBind()
    {
        currentBtns.SetActive(false);
        keyBindBtns.SetActive(true);
        currentBtns = keyBindBtns;
        keybind.Reset();
    }
    public void GoSetting()
    {
        currentBtns.SetActive(false);
        settingBtns.SetActive(true);
        currentBtns = settingBtns;
    }

    public void ReturnToRoot()
    {
        rootBtns.SetActive(true);
        currentBtns.SetActive(false);
        currentBtns = rootBtns;
    }

    public void ReturnToSetting()
    {
        settingBtns.SetActive(true);
        currentBtns.SetActive(false);
        currentBtns = settingBtns;
    }

    private void ResetBtns()
    {
        rootBtns.SetActive(true);
        settingBtns.SetActive(false);
        keyBindBtns.SetActive(false);
        currentBtns = rootBtns;
    }

    public void Continue()
    {
        MusicBattleRegulator.current.Continue();
    }
}
