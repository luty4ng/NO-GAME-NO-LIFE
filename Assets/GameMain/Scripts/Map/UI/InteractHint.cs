using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractHint : MonoBehaviour
{
    private Animator animator;
    private AudioSource[] audioSources;
    private AudioSource GetPopupSource() => audioSources[0];
    private AudioSource GetFlashDownSource() => audioSources[1];
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSources = GetComponents<AudioSource>();
    }

    public void Show()
    {
        GetPopupSource().Play();
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        GetFlashDownSource().Play();
        animator.SetTrigger("Hide");
    }
}
