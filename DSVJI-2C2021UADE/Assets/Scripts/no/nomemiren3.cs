using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class nomemiren3 : MonoBehaviour
{
    [SerializeField] [CanBeNull] private BaseEnemy boss;
    [SerializeField] private GameObject teleport;
    
    private void Update()
    {
        if (boss == null)
        {
            teleport.SetActive(true);
        }
    }
}
