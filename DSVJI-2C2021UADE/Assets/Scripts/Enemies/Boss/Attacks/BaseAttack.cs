using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies.Boss.Attacks 
{
    public abstract class BaseAttack : MonoBehaviour
    {
        protected Animator _animator;

        public virtual void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public virtual void Attack()
        {
            throw new InvalidOperationException("Not override of Attack method. Invoker: " + gameObject.name);
        }
    }
}
