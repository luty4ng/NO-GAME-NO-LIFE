using UnityEngine;
using GameKit;
public class Enemy : BattleEntity
{
    protected override void AddListener()
    {
        EventManager.instance.AddEventListener(EventConfig.E_Attack, () =>
        {
            animator.SetTrigger("Attack");
        });

        EventManager.instance.AddEventListener(EventConfig.E_BeAttacked, () =>
        {
            animator.SetTrigger("BeAttacked");
        });

        EventManager.instance.AddEventListener(EventConfig.E_Streak, () =>
        {
            animator.SetTrigger("Streak");
        });

        EventManager.instance.AddEventListener(EventConfig.E_StopStreak, () =>
        {
            animator.SetTrigger("StopStreak");
        });

        EventManager.instance.AddEventListener<bool>(EventConfig.Game_Pase, (bool isPause)=>{
            if(isPause)
                animator.speed = 0;
            else
                animator.speed = 1;
        });
    }
}