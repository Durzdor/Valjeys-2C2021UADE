using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GroundWaveController : MonoBehaviour
{
    [SerializeField]
    [Range(0, 4)]
    private float _expandSpeed;
    private Vector3 _expand;

    private readonly TimeSpan _ts = new TimeSpan(0, 0, 5);
    private readonly Stopwatch _sw = new Stopwatch();

    // Start is called before the first frame update
    void Start()
    {
        _expand = Vector2.one * _expandSpeed;
        _sw.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (_sw.Elapsed >= _ts)
            Destroy(gameObject);
        transform.localScale += _expand * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().TakeDamage(10);
            Destroy(gameObject);
        }
    }
}
