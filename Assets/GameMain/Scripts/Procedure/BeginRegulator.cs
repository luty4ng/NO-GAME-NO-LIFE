using System;
using DG.Tweening;
using GameKit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeginRegulator : Regulator<BeginRegulator>
{
    private bool fadeInComplete = false;

    public Image beginImage;

    private void Start()
    {
        beginImage.DOFade(1, 3f).OnComplete(() =>
        {
            fadeInComplete = true;
        });
    }

    private void Update()
    {
        if (fadeInComplete && Input.anyKey)  // if the player moves
        {
            beginImage.DOFade(0, 3f).OnComplete(() =>
            {
                SceneManager.LoadScene("S_Procedure");
            });
        }
    }
}
