using UnityEngine;

namespace Assets.Scripts.Enemies.Boss.Attacks 
{
    public class GroundWave : BaseAttack
    {
        [SerializeField]
        private GameObject _prefab;
        [SerializeField]
        private Transform _spawnPoint;
        
        public override void Attack()
        {
            _animator.ResetTrigger("Spell");
            _animator.SetTrigger("Spell");
            Instantiate(_prefab, _spawnPoint);
        }
    }
}
