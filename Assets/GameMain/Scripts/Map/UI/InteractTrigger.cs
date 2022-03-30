using System;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    private Interactable currentInteractable;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && currentInteractable != null)
        {
            currentInteractable.Action();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactableComponent = other.GetComponent<Interactable>();
        if (interactableComponent != null)
        {
            interactableComponent.OnEnter();
            if (currentInteractable != null)
            {
                Debug.LogWarning("Warning: Two interact triggers enter at the same time.");
            }
            currentInteractable = interactableComponent;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactableComponent = other.GetComponent<Interactable>();
        if (interactableComponent != null && interactableComponent == currentInteractable)
        {
            interactableComponent.OnExit();
            currentInteractable = null;
        }
    }
}