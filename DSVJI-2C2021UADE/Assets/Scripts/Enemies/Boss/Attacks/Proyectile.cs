using System;
using System.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.Enemies.Boss.Attacks
{
    public class Proyectile : MonoBehaviour
    {
        private Vector3 _dir;

        private readonly TimeSpan _ts = new TimeSpan(0, 0, 5);
        private readonly Stopwatch _sw = new Stopwatch();

        public void SetUp(Vector3 dir, float speed = 2f)
        {
            _sw.Start();
            _dir = dir * speed;
        }

        void Update()
        {
            if (_sw.Elapsed >= _ts)
                Destroy(gameObject);
            transform.position += _dir * Time.deltaTime;    
        }

        void LateUpdate()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
