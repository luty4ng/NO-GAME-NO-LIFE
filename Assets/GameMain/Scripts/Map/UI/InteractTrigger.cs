using System;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    private Interactable _currentInteractable;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.Action();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Interactable interactableComponent = other.GetComponent<Interactable>();
        if (interactableComponent != null)
        {
            interactableComponent.OnEnter();
            _currentInteractable = interactableComponent;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Interactable interactableComponent = other.GetComponent<Interactable>();
        if (interactableComponent != null)
        {
            interactableComponent.OnExit();
            _currentInteractable = null;
        }
    }
}