using System;
using System.Collections;
using GameKit;
using UnityEngine;
using UnityEngine.Events;

public class EndCreditController : MonoBehaviour
{
    public GameObject panel;
    
    private Animator _animator;

    private void Start()
    {
        _animator = panel.GetComponent<Animator>();
        EventManager.instance.AddEventListener(EventConfig.SHOW_END_CREDIT, Show);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveEventListener(EventConfig.SHOW_END_CREDIT, Show);
    }

    private void Show()
    {
        panel.SetActive(true);
    }

    private void Update()
    {
        if (panel.activeSelf && Input.anyKeyDown)
        {
            _animator.SetTrigger("Hide");
            StartCoroutine(DelayedExcute(() =>
            {
                MapGlobals.CurrentLevel = -1;
                MapGlobals.SwitchToMain();
            }, 5f));
        }
    }
    
    protected IEnumerator DelayedExcute(UnityAction action, float t)
    {
        yield return new WaitForSeconds(t);
        action?.Invoke();
    }
}