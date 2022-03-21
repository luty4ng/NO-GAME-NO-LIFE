using UnityEngine;
using GameKit;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
public class Protagonist_Flip : BattleEntity
{
    private Image healthBar;
    public float initHealth = 1000;
    public Protagonist protagonist;
    private bool isLoop = false;
    protected override void AddListener()
    {
        EventManager.instance.AddEventListener(EventConfig.E_Attack, () =>
        {
            animator.SetTrigger("Attack");
        });



        EventManager.instance.AddEventListener<float, bool>(EventConfig.E_BeAttacked, (float damage, bool isStreak) =>
        {
            animator.SetTrigger("BeAttacked");
            BeAttack(damage);
            if (isStreak)
            {
                isLoop = true;
                StartCoroutine(LoopByPeriod(() =>
                {
                    BeAttack(damage);
                }));
            }
        });

        EventManager.instance.AddEventListener(EventConfig.E_Streak, () =>
        {
            animator.SetTrigger("Streak");
        });

        EventManager.instance.AddEventListener(EventConfig.E_StopStreak, () =>
        {
            animator.SetTrigger("StopStreak");
        });
        EventManager.instance.AddEventListener(EventConfig.E_StopLoop, () =>
        {
            isLoop = false;
        });

        EventManager.instance.AddEventListener<bool>(EventConfig.Game_Pase, PauseAnimator);
    }
    protected override void Initialize()
    {
        healthBar = GameObject.Find("EnemyHealth").GetComponent<Image>();
        protagonist = GameObject.Find("Protagonist").GetComponent<Protagonist>();
        health = initHealth;
    }
    protected override void BeAttack(float multipier)
    {
        health = Mathf.Max(health - protagonist.damage * multipier, 0);
        healthBar.fillAmount = (health / initHealth);
        if (health <= 0)
            EventManager.instance.EventTrigger(EventConfig.E_Failed);
    }
    private void OnValidate()
    {
        protagonist.enemy = this;
    }

    private void PauseAnimator(bool isPause)
    {
        if (isPause)
            animator.speed = 0;
        else
            animator.speed = 1;
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveEventListener<bool>(EventConfig.Game_Pase, PauseAnimator);
        EventManager.instance.ClearEventListener(EventConfig.E_StopStreak);
        EventManager.instance.ClearEventListener(EventConfig.E_Streak);
        EventManager.instance.ClearEventListener(EventConfig.E_Attack);
        EventManager.instance.ClearEventListener<float, bool>(EventConfig.E_BeAttacked);
        EventManager.instance.ClearEventListener(EventConfig.E_StopLoop);
    }

    private IEnumerator LoopByPeriod(UnityAction evt = null)
    {
        while (isLoop)
        {
            evt?.Invoke();
            Debug.Log("Enemy Long BeAttack");
            yield return new WaitForSeconds(0.3f);
        }
    }
}