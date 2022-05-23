using System;
using System.Collections;
using System.Collections.Generic;
using GameKit;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private AudioSource _audioSource;
    private float _defaultScale;
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _defaultScale = transform.localScale.x;  // used for changing character direction
        
        EventManager.instance.AddEventListener<bool>(EventConfig.Game_Pase, UpdateAudioSourceStatus);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveEventListener<bool>(EventConfig.Game_Pase, UpdateAudioSourceStatus);
    }

    private void Update()
    {
        UpdatePlayerMovement();
    }

    private void UpdateAudioSourceStatus(bool pause)
    {
        if (pause)
        {
            _audioSource.Pause();
        }
        else
        {
            _audioSource.Play();
        }
    }

    private void UpdatePlayerMovement()
    {
        if (MapRegulator.current.DialogUIActive)
        {
            _rigidbody2D.velocity = new Vector2(0, 0);
        }
        else
        {
            var direction = Input.GetAxisRaw("Horizontal");  // get input direction
            _rigidbody2D.velocity = new Vector2(speed * direction, 0);  // set horizontal movement
            if (direction != 0)
            {
                // change face direction
                float newXScale = _defaultScale * direction;
                transform.localScale = new Vector3(newXScale, _defaultScale, 1);
            
                // trigger animation
                _animator.SetTrigger("Walk");
            }
            else
            {
                // trigger animation
                _animator.SetTrigger("Stand");
            }
        }
    }
}
