using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using GameKit;

[RequireComponent(typeof(Animator))]
public abstract class Entity : MonoBehaviour
{
    protected Image healthBar;
    protected Animator animator;
    protected float health;
    protected bool isActing;
    public float fullHealth;
    public float damage;

    public float Health
    {
        get
        {
            return health;
        }
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        EventManager.instance.AddEventListener<bool>(EventConfig.Game_Pase, PauseAnimator);
        OnStart();
    }
    protected virtual void OnStart() { }
    protected virtual void OnDamaged(float damage) { }
    protected IEnumerator DelayTigger(string name, float t)
    {
        yield return new WaitForSeconds(t);
        animator.SetTrigger(name);
    }
    protected IEnumerator LoopByPeriod(UnityAction evt = null, float period = 0.3f)
    {
        while (isActing)
        {
            evt?.Invoke();
            Debug.Log("Loop By Period");
            yield return new WaitForSeconds(period);
        }
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
    }
}