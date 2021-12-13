using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies.Boss.Attacks 
{
    public class GroundWave : BaseAttack
    {
        private GameObject _prefab;
        
        public override void Attack()
        {
            _animator.SetTrigger("Spell");
            Instantiate(_prefab, transform.position, transform.rotation);
        }
    }
}
