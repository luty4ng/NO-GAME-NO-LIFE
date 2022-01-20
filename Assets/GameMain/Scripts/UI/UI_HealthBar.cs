using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKit;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public sealed class UI_HealthBar : UIGroup
{
    public Image myHealth;
    public Image enemyHealth;
    protected override void Start()
    {
        UIManager.instance.RegisterUI(this as UIGroup);
    }
}
