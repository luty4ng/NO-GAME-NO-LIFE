using System;
using GameKit;
using UnityEngine;

// use this class on popup windows that opens by an event and closed by user clicking [L]
public class PopupController : MonoBehaviour
{
    public GameObject panel;  // the panel to be shown / hidden, should be inactive by default
    public string showEvent;  // the name of the event to triggers the popup

    private bool _toShow;

    private void Start()
    {
        EventManager.instance.AddEventListener(showEvent, OnShowPanel);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveEventListener(showEvent, OnShowPanel);
    }

    private void OnShowPanel()
    {
        if (!panel.activeSelf)
        {
            _toShow = true;
        }
    }

    private void LateUpdate()
    {
        if (_toShow)
        {
            panel.SetActive(true);
            MapGlobals.DialogUIActive = true;
            _toShow = false;
        }
        else if (panel.activeSelf && Input.GetKeyDown(KeyCode.L))
        {
            panel.SetActive(false);
            MapGlobals.DialogUIActive = false;
        }
    }
}