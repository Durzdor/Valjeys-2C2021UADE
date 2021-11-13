using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleProp : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private GameObject _parent;
    [SerializeField] private int _powerLevel;
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private GameObject[] _props;
#pragma warning restore 649

    #endregion
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo entro a mi dominio!!");
        if (other.CompareTag("PlayerProjectile"))
        {
            Debug.Log("Es un proyectil!!!");
            var projectile = other.GetComponent<Projectile>();
            if (projectile._powerLevel >= _powerLevel)
            {
                Debug.Log("Y es muy poderoso!!!");
                Destroy(projectile.gameObject);
                Explode();          
            }
        }
    }

    private void Explode()
    {
        _explosion.Play();
        for (int i = 0; i < _props.Length; i++)
        {
            Destroy(_props[i]);
        }
        Invoke(nameof(SelfDestroy), 2.5f);
    }

    private void SelfDestroy()
    {
        Destroy(_parent);
    }
}
