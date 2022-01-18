using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class BattleEntity : MonoBehaviour
{
    protected float health = 100;
    protected Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        AddListener();
        Initialize();
    }
    protected virtual void AddListener() {}
    protected virtual void Initialize() {}
}