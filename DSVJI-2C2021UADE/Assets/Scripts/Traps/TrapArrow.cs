using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrow : MonoBehaviour
{
    [SerializeField] private float lifespan = 3f;
    [SerializeField] private float speed = 8f;

    private void Update()
    {
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }

        lifespan -= Time.deltaTime;
        
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }
}
