using UnityEngine;
using UnityEngine.UI;
using GameKit;
public class Protagonist_Flip : BattleEntity
{
    private Image healthBar;
    public float initHealth = 1000;
    public Protagonist protagonist;
    protected override void AddListener()
    {
        EventManager.instance.AddEventListener(EventConfig.E_Attack, () =>
        {
            animator.SetTrigger("Attack");
        });

        EventManager.instance.AddEventListener<float>(EventConfig.E_BeAttacked, (float damage) =>
        {
            animator.SetTrigger("BeAttacked");
            BeAttack(damage);
        });

        EventManager.instance.AddEventListener(EventConfig.E_Streak, () =>
        {
            Debug.Log("E_Streak");
            animator.SetTrigger("Streak");
        });

        EventManager.instance.AddEventListener(EventConfig.E_StopStreak, () =>
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
    }
    private void OnValidate()
    {
        protagonist.enemy = this;
    }
}