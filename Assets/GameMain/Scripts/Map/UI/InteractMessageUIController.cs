using System;
using GameKit;
using UnityEngine;
using UnityEngine.UI;

public class InteractMessageUIController : MonoBehaviour
{
    public GameObject interactMessagePanel;

    private Interactable _currentInteractable;
    
    private void Start()
    {
        EventManager.instance.AddEventListener<Interactable>(EventConfig.SHOW_INTERACT_MESSAGE, ShowInteractMessage);
        EventManager.instance.AddEventListener(EventConfig.HIDE_INTERACT_MESSAGE, HideInteractMessage);
    }

    private void ShowInteractMessage(Interactable interactable)
    {
        interactMessagePanel.gameObject.SetActive(true);
        GetComponentInChildren<Text>().text = interactable.message;
        _currentInteractable = interactable;
    }
    
    private void HideInteractMessage()
    {
        interactMessagePanel.gameObject.SetActive(false);
        _currentInteractable = null;
    }

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
}