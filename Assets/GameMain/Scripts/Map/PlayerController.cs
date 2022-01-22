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
    private float _defaultScale;

    private List<Collision2D> colliders = new List<Collision2D>();
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _defaultScale = transform.localScale.x;  // used for changing character direction
    }

    private void Update()
    {
        UpdatePlayerMovement();
    }

    private void UpdatePlayerMovement()
    {
        if (MapGlobalStatus.DialogUIActive)
        {
            _rigidbody2D.velocity = new Vector2(0, 0);
        }
        else
        {
            float direction = Input.GetAxisRaw("Horizontal");  // get input direction
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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Interactable interactableComponent = other.GetComponent<Interactable>();
        if (interactableComponent != null)
        {
            EventManager.instance.EventTrigger<Interactable>(EventConfig.SHOW_INTERACT_MESSAGE, interactableComponent);

            // colliders.Add(other);
            // if (colliders.Count == 1)
            // {
            //     EventManager.instance.EventTrigger<string>(EventConfig.SHOW_INTERACT_MESSAGE, interactableComponent.message);
            // }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Interactable interactableComponent = other.GetComponent<Interactable>();
        if (interactableComponent != null)
        {
            EventManager.instance.EventTrigger(EventConfig.HIDE_INTERACT_MESSAGE);
            // if (other == colliders[0])
            // {
            //     EventManager.instance.EventTrigger(EventConfig.HIDE_INTERACT_MESSAGE);
            // }
            // else
            // {
            //     EventManager.instance.EventTrigger<string>(EventConfig.SHOW_INTERACT_MESSAGE, interactableComponent.message);
            // }
            //
            // colliders.Remove(other);
        }
    }
}
