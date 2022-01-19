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
    public Image myVirtual;
    public Image enemyHealth;
    public Image enemyVirtual;
    public float reduceSpeed = 0.01f;
    protected override void Start()
    {
        UIManager.instance.RegisterUI(this as UIGroup);
    }
    private void Update()
    {
        if (enemyVirtual.fillAmount != enemyHealth.fillAmount)
        {
            enemyVirtual.fillAmount -= reduceSpeed * Time.deltaTime;
            if(Mathf.Abs(enemyVirtual.fillAmount - enemyHealth.fillAmount) < 0.01f)
                enemyVirtual.fillAmount = enemyHealth.fillAmount;
        }

        if (myVirtual.fillAmount != myHealth.fillAmount)
        {
            myVirtual.fillAmount -= reduceSpeed * Time.deltaTime;
            if(Mathf.Abs(myVirtual.fillAmount - myHealth.fillAmount) < 0.01f)
                myVirtual.fillAmount = myHealth.fillAmount;
        }  
    }
}
