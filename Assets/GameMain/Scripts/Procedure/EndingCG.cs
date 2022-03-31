using UnityEngine;
using UnityEngine.UI;
using GameKit;
using DG.Tweening;

public class EndingCG : MonoBehaviour
{
    public Image endingPic;

    private void Start()
    {
        EventManager.instance.AddEventListener(EventConfig.FINAL_CG, () =>
        {
            endingPic.enabled = true;
            endingPic.DOFade(1, 4f).OnComplete(() =>
            {
                endingPic.DOFade(1, 3f).OnComplete(() =>
                {
                    MusicBattleRegulator.current.CompleteLevel();
                });
            });
        });
    }
}