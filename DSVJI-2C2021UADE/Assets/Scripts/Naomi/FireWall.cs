using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWall : MonoBehaviour
{
    [SerializeField] private float _lifespan = 3f;
    [SerializeField] private float _tickPerSecond = 4f;
    [SerializeField] private Collider _collider;

    private Vector3 _direction;
    private float _tickTime;
    // private List<Collider> _enemyColliders = new List<Collider>();
    private bool _enemyCollide;
    private float _firewallDamage;
    
    public void Init(Vector3 initialPosition, float damage)
    {
        _firewallDamage = damage;
        transform.position = initialPosition;
    }
    
    void Update()
    {
        if (_lifespan <= 0)
        {
            Destroy(gameObject);
        }

        _lifespan -= Time.deltaTime;

        // if I had enemies in my collider, start tick timer and disable collider
        if (_enemyCollide)
        {
            print("Iniciando ticks de la pared de fuego");
            _tickTime += Time.deltaTime;
            _collider.enabled = false;
        }

        // if timer reaches tick max time, clear tick counter
        if (_tickTime >= 1/_tickPerSecond)
        {
            print("Se termino el tick");
            _tickTime = 0;
            _enemyCollide = false;
            _collider.enabled = true;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        print("Algo entro entro en mis dominios!!!  :"  + other.tag);
        
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                var enemyHealth = enemy.Health;
                enemyHealth.TakeDamage(_firewallDamage);
            }
            print("Un enemigo entro en mis dominios!!");
            _enemyCollide = true;
        }
    }
    
}
