using UnityEngine;
using GameKit;
public class Protagonist : BattleEntity
{
    protected override void OnStart()
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
    }
}