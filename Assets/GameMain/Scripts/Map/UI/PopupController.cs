using System;
using GameKit;
using UnityEngine;

// use this class on popup windows that opens by an event and closed by user clicking [L]
public class PopupController : MonoBehaviour
{
    public GameObject panel;  // the panel to be shown / hidden, should be inactive by default
    public string showEvent;  // the name of the event to triggers the popup

    private AudioSource _audioSource;
    private bool _toShow;

    private void Start()
    {
        var sources = GetComponents<AudioSource>();
        if (sources.Length > 0)
        {
            _audioSource = sources[0];
        }
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
            _audioSource.Play();
            panel.SetActive(true);
            MapGlobals.DialogIn();
            _toShow = false;
        }
        else if (panel.activeSelf && Input.GetKeyDown(KeyCode.L))
        {
            panel.SetActive(false);
            MapGlobals.DialogOut();
        }
    }
}