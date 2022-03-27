using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    void Start()
    {
        _animator.Play("spawn");
        _rigidbody.useGravity = false;
    }

}
