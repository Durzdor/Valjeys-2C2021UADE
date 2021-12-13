using System;
using UnityEngine;

namespace Assets.Scripts.Enemies.Boss.Attacks
{
    public class DirectRangeAttack : BaseAttack
    {
        private GameObject _prefab;
        public override void Attack()
        {
            Instantiate(_prefab).GetComponent<Proyectile>().SetUp(transform.forward);
        }
    }
}
