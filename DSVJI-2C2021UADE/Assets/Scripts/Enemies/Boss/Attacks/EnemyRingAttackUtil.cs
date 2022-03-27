using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRingAttackUtil : MonoBehaviour
{
    [SerializeField] private GameObject _parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(_parent);
        }
    }
}
