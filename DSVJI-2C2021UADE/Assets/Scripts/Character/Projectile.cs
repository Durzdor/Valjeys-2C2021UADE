﻿using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifespan = 3f;
    [SerializeField] private float _speed = 8f;
    public int _powerLevel;
    
    private Vector3 _direction;
    

    public void Init(Vector3 direction, Vector3 initialPosition)
    {
        transform.position = initialPosition;
        _direction = direction;
    }
    
    void Update()
    {
        if (_lifespan <= 0)
        {
            Destroy(gameObject);
        }

        _lifespan -= Time.deltaTime;
        
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        var collider = GetComponent<Collider>();
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(collider.transform.position, 1);
            
    }

    private void OnCollisionEnter(Collision other)
    {
        print("collision");
        Destroy(gameObject);
    }
}
