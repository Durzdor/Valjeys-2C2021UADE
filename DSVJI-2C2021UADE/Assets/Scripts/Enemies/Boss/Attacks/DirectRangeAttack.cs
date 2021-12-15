using UnityEngine;

namespace Assets.Scripts.Enemies.Boss.Attacks
{
    public class DirectRangeAttack : BaseAttack
    {
        [SerializeField]
        private GameObject _prefab;
        [SerializeField]
        private float _proyectileSpeed;

        public override void Attack()
        {
            Instantiate(_prefab).GetComponent<Proyectile>().SetUp(transform.forward, _proyectileSpeed);
        }
    }
}
