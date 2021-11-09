using UnityEngine;

/// <summary>
/// MAXI, TE ODIOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
/// </summary>
public class PhysicsFix : MonoBehaviour
{
    private Vector3 _rot;
    private Vector3 _fixedVelocity;
    Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rot = new Vector3(0, 0, 0);
        _fixedVelocity = new Vector3();
    }
    private void LateUpdate()
    {
        _rot.y = transform.localEulerAngles.y;
        transform.localEulerAngles = _rot;
        _fixedVelocity = _rb.velocity;
        _fixedVelocity.y = Mathf.Clamp(_fixedVelocity.y, float.MinValue, 1f);
        _rb.velocity = _fixedVelocity;
    }
}