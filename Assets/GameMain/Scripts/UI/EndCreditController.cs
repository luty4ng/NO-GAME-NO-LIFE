using System;
using System.Collections;
using GameKit;
using UnityEngine;
using UnityEngine.Events;

public class EndCreditController : MonoBehaviour
{
    public GameObject panel;
    
    private Animator animator;

    private void Start()
    {
        animator = panel.GetComponent<Animator>();
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
        if (Input.anyKeyDown && panel.activeSelf && animator.GetCurrentAnimatorStateInfo(0).IsName("End_Credit_Sustain"))
        {
            animator.SetTrigger("Hide");
            StartCoroutine(DelayedExecute(() =>
            {
                MapGlobals.CurrentLevel = -1;
                MapGlobals.SwitchToMain();
            }, 5f));
        }
    }
    
    protected IEnumerator DelayedExecute(UnityAction action, float t)
    {
        yield return new WaitForSeconds(t);
        action?.Invoke();
    }
}