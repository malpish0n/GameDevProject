using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsController : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovement _movement;

    private void Start()
    {
        GetReferences();
    }

    public void LoopSlideAnimation()
    {
        if (_movement.SlopeCheck())
        {
            _animator.speed = 0f;
        }
        else if (!_movement.SlopeCheck())
        {
            _animator.speed = 1f;
        }
    }

    private void GetReferences()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponentInParent<PlayerMovement>();
    }
}
