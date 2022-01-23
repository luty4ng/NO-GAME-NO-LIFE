using System;
using GameKit;
using UnityEngine;

public class Portal : Interactable
{
    private AudioSource _audioSource;

    private void Start()
    {
        var sources = GetComponents<AudioSource>();
        if (sources.Length > 0)
        {
            _audioSource = sources[0];
        }
    }

    public override void OnEnter()
    {
        _audioSource?.Play();
    }
}