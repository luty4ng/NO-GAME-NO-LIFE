using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractHint : MonoBehaviour
{
    private Animator _animator;
    private AudioSource[] _audioSources;
    private AudioSource GetPopupSource() => _audioSources[0];
    private AudioSource GetFlashDownSource() => _audioSources[1];
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSources = GetComponents<AudioSource>();
    }

    public void Show()
    {
        GetPopupSource().Play();
        _animator.SetTrigger("Show");
    }

    public void Hide()
    {
        GetFlashDownSource().Play();
        _animator.SetTrigger("Hide");
    }
}
