using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public virtual void OnEnter() { }
    
    public virtual void OnExit() { }

    public virtual void Action() { }
}