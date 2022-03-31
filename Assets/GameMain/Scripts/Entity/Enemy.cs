using UnityEngine;
using GameKit;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
public class Enemy : Entity
{
    public Protagonist protagonist;
    private Coroutine damageCoroutine;
    public void Attack() => animator.SetTrigger("Attack");
    public void Streak() => animator.SetTrigger("Streak");
    public void StopStreak()
    {
        animator.SetTrigger("StopStreak");
        Debug.Log("Enemy Stop Streak");
    }
    public void BeAttack(float multipier)
    {
        animator.SetTrigger("BeAttack");
        OnDamaged(multipier);
    }

    public void BeStreak(float multipier)
    {
        animator.SetTrigger("BeStreak");
        OnDamaged(multipier);
        damageCoroutine = StartCoroutine(LoopByPeriod(() =>
        {
            OnDamaged(multipier);
        }));
    }
    public void StopBeStreak()
    {
        animator.SetTrigger("StopBeStreak");
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
    }
    protected override void OnStart()
    {
        healthBar = GameObject.Find("EnemyHealth").GetComponent<Image>();
        protagonist = GameObject.Find("Protagonist").GetComponent<Protagonist>();
        health = fullHealth;
    }
    protected override void OnDamaged(float multipier)
    {
        health = Mathf.Max(health - protagonist.damage * multipier, 0);
        healthBar.fillAmount = (health / fullHealth);
        if (health <= 0)
            EventManager.instance.EventTrigger(EventConfig.E_Failed);
    }
    private void OnValidate()
    {
        protagonist.enemy = this;
    }
}