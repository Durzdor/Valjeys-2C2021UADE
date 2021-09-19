using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbRotation : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private float angle = 1;
#pragma warning restore 649
    #endregion
    
    private void FixedUpdate()
    {
        var t = transform;
        var pos = t.position;
        transform.RotateAround(pos,t.forward,angle * Time.deltaTime);
        transform.RotateAround(pos,t.right,-angle* Time.deltaTime);
        transform.RotateAround(pos,t.up,angle* Time.deltaTime);
    }
}