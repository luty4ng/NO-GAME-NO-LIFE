using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractHint : MonoBehaviour
{
    private Animator _animator;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Show()
    {
        _animator.SetTrigger("Show");
    }

    public void Hide()
    {
        _animator.SetTrigger("Hide");
    }
}
