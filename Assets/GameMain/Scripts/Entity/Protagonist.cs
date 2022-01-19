using UnityEngine;
using GameKit;
using UnityEngine.UI;
public class Protagonist : BattleEntity
{
    private Image healthBar;
    public float initHealth = 1000;
    public BattleEntity enemy;
    protected override void Initialize()
    {
        healthBar = GameObject.Find("MyHealth").GetComponent<Image>();
        health = initHealth;
    }
    protected override void AddListener()
    {
        EventManager.instance.AddEventListener(EventConfig.P_Attack, () =>
        {
            animator.SetTrigger("Attack");
        });

        EventManager.instance.AddEventListener(EventConfig.P_Defense, () =>
        {
            animator.SetTrigger("Defense");
        });

        EventManager.instance.AddEventListener<float>(EventConfig.P_BeAttacked, (float damage) =>
        {
            animator.SetTrigger("BeAttacked");
            BeAttack(damage);
        });

        EventManager.instance.AddEventListener(EventConfig.P_Streak, () =>
        {
            animator.SetTrigger("Streak");
        });

        EventManager.instance.AddEventListener(EventConfig.P_StopStreak, () =>
        {
            animator.SetTrigger("StopStreak");
        });
        EventManager.instance.AddEventListener(EventConfig.P_StreakDefense, () =>
        {
            animator.SetTrigger("StreakDefense");
        });
        EventManager.instance.AddEventListener(EventConfig.P_StopStreakDefense, () =>
        {
            animator.SetTrigger("StopStreakDefense");
        });

        EventManager.instance.AddEventListener<bool>(EventConfig.Game_Pase, PauseAnimator);
    }

    protected override void BeAttack(float multipier)
    {
        health = Mathf.Max(health - enemy.damage / multipier, 0);
        healthBar.fillAmount = (health / initHealth);
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
        EventManager.instance.ClearEventListener(EventConfig.P_StopStreakDefense);
        EventManager.instance.ClearEventListener(EventConfig.P_StopStreak);
        EventManager.instance.ClearEventListener(EventConfig.P_Streak);
        EventManager.instance.ClearEventListener(EventConfig.P_StreakDefense);
        EventManager.instance.ClearEventListener(EventConfig.P_Attack);
        EventManager.instance.ClearEventListener(EventConfig.P_Defense);
        EventManager.instance.ClearEventListener<float>(EventConfig.P_BeAttacked);
    }

}