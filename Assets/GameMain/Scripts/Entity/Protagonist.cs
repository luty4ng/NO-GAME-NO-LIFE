using UnityEngine;
using GameKit;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
public class Protagonist : Entity
{
    public Enemy enemy;
    private bool isEnemyFail = false;
    private Coroutine damageCoroutine;
    public void Attack() => animator.SetTrigger("Attack");
    public void Defense() => animator.SetTrigger("Defense");
    public void Streak() => animator.SetTrigger("Streak");
    public void StopStreak() => animator.SetTrigger("StopStreak");
    public void DefenseStreak() => animator.SetTrigger("StreakDefense");
    public void StopDefenseStreak() => animator.SetTrigger("StopDefenseStreak");
    public void EnemyFail() => isEnemyFail = true;
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
        healthBar = GameObject.Find("MyHealth").GetComponent<Image>();
        enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        health = fullHealth;
        EventManager.instance.AddEventListener(EventConfig.E_Failed, EnemyFail);
    }

    protected override void OnDamaged(float multipier)
    {
        if (!isEnemyFail)
        {
            health = Mathf.Max(health - enemy.damage / multipier, 0);
        }
        healthBar.fillAmount = (health / fullHealth);
    }

    private void PauseAnimator(bool isPause)
    {
        if (isPause)
            animator.speed = 0;
        else
            animator.speed = 1;
    }
}