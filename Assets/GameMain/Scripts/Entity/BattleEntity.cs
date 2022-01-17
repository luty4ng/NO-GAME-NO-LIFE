using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class BattleEntity : MonoBehaviour
{
    public float health = 100;
    protected Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        OnStart();
    }

    protected virtual void OnStart()
    {

    }
}