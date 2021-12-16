using UnityEngine;

namespace Assets.Scripts.Enemies.Boss.Attacks
{
    public class DirectRangeAttack : BaseAttack
    {
        [SerializeField]
        private GameObject _prefab;
        [SerializeField]
        private float _proyectileSpeed;
        [SerializeField]
        private Transform _spawnPoint;

        private Blackboard _memory;

        public override void Start()
        {
            _memory = GetComponentInParent<Blackboard>();
            base.Start();
        }

        public override void Attack()
        {
            Vector3 playerPos = (Vector3)_memory.Get("PlayerPosition");
            Vector3 dir = playerPos - _spawnPoint.position;
            Instantiate(_prefab, _spawnPoint).GetComponent<Proyectile>().SetUp(dir, _proyectileSpeed);
        }
    }
}
