using UnityEngine;
using GameKit;
using UnityEngine.UI;
public class Protagonist : BattleEntity
{
    private Image healthBar;
    public float initHealth = 100;
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

        EventManager.instance.AddEventListener(EventConfig.P_BeAttacked, () =>
        {
            animator.SetTrigger("BeAttacked");
        });

        EventManager.instance.AddEventListener(EventConfig.P_Streak, () =>
        {
            animator.SetTrigger("Streak");
        });

        EventManager.instance.AddEventListener(EventConfig.P_StopStreak, () =>
        {
            animator.SetTrigger("StopStreak");
        });

        EventManager.instance.AddEventListener<bool>(EventConfig.Game_Pase, (bool isPause) =>
        {
            if (isPause)
                animator.speed = 0;
            else
                animator.speed = 1;
        });
    }
}