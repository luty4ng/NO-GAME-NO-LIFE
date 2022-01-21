using System;
using GameKit;
using UnityEditor.VersionControl;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string message;

    public virtual void Action()
    {
        throw new NotImplementedException();
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     Debug.Log("Collide");
    //     if (other.gameObject.tag == "Player")
    //     {
    //         Debug.Log("Collide with player");
    //
    //         // colliders.Add(other);
    //         // if (colliders.Count == 1)
    //         // {
    //         //     EventManager.instance.EventTrigger<string>(EventConfig.SHOW_INTERACT_MESSAGE, interactableComponent.message);
    //         // }
    //     }
    // }
    //
    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         Debug.Log("End collide with player");
    //         EventManager.instance.EventTrigger(EventConfig.HIDE_INTERACT_MESSAGE);
    //         // if (other == colliders[0])
    //         // {
    //         //     EventManager.instance.EventTrigger(EventConfig.HIDE_INTERACT_MESSAGE);
    //         // }
    //         // else
    //         // {
    //         //     EventManager.instance.EventTrigger<string>(EventConfig.SHOW_INTERACT_MESSAGE, interactableComponent.message);
    //         // }
    //         //
    //         // colliders.Remove(other);
    //     }
    // }
}