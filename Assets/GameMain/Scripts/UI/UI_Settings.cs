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
    public Slider volumeSlider;
    protected override void OnStart()
    {
        base.OnStart();
        volumeSlider = GetUIComponent<Slider>("Sld_Volume");
    }
    protected override FindType findType { get { return FindType.All; } }
    public override void Show(UnityAction callback = null)
    {
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
}
