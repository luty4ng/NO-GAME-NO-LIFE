using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKit;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public sealed class UI_Ending : UIGroup
{
    public GameObject victory;
    public GameObject defeat;
    protected override void OnStart()
    {
        base.OnStart();
        victory.SetActive(false);
        defeat.SetActive(false);
    }
    public bool isWin = true;
    protected override FindType findType { get { return FindType.SubChildren; } }
    public override void Show(UnityAction callback = null)
    {
        CheckWin();
        
        canvasGroup.alpha = 1;
        this.transform.localScale = Vector3.one * 0.5f;
        this.transform.DOScale(Vector3.one, 0.5f);
        this.gameObject.SetActive(true);
        callback?.Invoke();
    }

    public override void Hide(UnityAction callback = null)
    {
        this.transform.localScale = Vector3.one;
        this.transform.DOScale(Vector3.one * 0.5f, 0.5f).OnComplete(() =>
        {
            canvasGroup.alpha = 0;
        });
        this.gameObject.SetActive(false);
        callback?.Invoke();
    }

    private void CheckWin()
    {
        if (isWin)
        {
            MusicBattleRegulator.current.PlaySoundClip(MusicBattleRegulator.current.audioMono.BATTLE_WIN);
            victory.SetActive(true);
            defeat.SetActive(false);
        }
        else
        {
            MusicBattleRegulator.current.PlaySoundClip(MusicBattleRegulator.current.audioMono.BATTLE_LOST);
            defeat.SetActive(true);
            victory.SetActive(false);
        }
    }
}
