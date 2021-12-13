using UnityEngine;

namespace Assets.Scripts.Enemies.Boss.Attacks
{
    public class Proyectile : MonoBehaviour
    {
        private Vector3 _dir;
        public void SetUp(Vector3 dir, float speed = 2f)
        {
            _dir = dir * speed;    
        }

        void Update()
        {
            transform.position += _dir;    
        }
    }
}
