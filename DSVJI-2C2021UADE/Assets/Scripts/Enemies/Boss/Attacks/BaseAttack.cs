using System;
using UnityEngine;

namespace Assets.Scripts.Enemies.Boss.Attacks 
{
    public abstract class BaseAttack : MonoBehaviour
    {
        [SerializeField]
        protected Animator _animator;

        //public virtual void Start()
        //{
        //    _animator = GetComponentInParent<Animator>();
        //}

        public virtual void Attack()
        {
            throw new InvalidOperationException("Not override of Attack method. Invoker: " + gameObject.name);
        }
    }
}
