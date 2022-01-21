using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public abstract class BattleEntity : MonoBehaviour
{
    protected float health = 100;
    public float damage = 8;
    protected Animator animator;
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
        AddListener();
        Initialize();
    }
    protected virtual void AddListener() { }
    protected virtual void Initialize() { }
    protected virtual void BeAttack(float damage) { }
    protected IEnumerator DelayTigger(string name, float t)
    {
        yield return new WaitForSeconds(t);
        animator.SetTrigger(name);
    }

}