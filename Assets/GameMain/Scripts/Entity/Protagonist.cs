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
            // StartCoroutine(DelayTigger("StopStreak", 0.1f));
        });

        EventManager.instance.AddEventListener<bool>(EventConfig.Game_Pase, (bool isPause) =>
        {
            if (isPause)
                animator.speed = 0;
            else
                animator.speed = 1;
        });
    }

    protected override void BeAttack(float multipier)
    {
        // Debug.Log(enemy.damage * multipier);
        health = Mathf.Max(health - enemy.damage / multipier, 0);
        healthBar.fillAmount = (health / initHealth);
        Debug.Log(health);
    }


}