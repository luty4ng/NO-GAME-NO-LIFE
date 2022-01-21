using System;
using GameKit;
using UnityEngine;
using UnityEngine.UI;

public class InteractMessageUIController : MonoBehaviour
{
    public GameObject interactMessagePanel;
    
    private void Start()
    {
        EventManager.instance.AddEventListener<string>(EventConfig.SHOW_INTERACT_MESSAGE, ShowInteractMessage);
        EventManager.instance.AddEventListener(EventConfig.HIDE_INTERACT_MESSAGE, HideInteractMessage);
    }

    private void ShowInteractMessage(string message)
    {
        interactMessagePanel.gameObject.SetActive(true);
        GetComponentInChildren<Text>().text = message;
    }
    
    private void HideInteractMessage()
    {
        interactMessagePanel.gameObject.SetActive(false);
    }
}