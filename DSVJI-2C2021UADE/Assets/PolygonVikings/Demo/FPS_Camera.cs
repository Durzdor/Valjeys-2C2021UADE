using UnityEngine;

public class FPS_Camera : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float horizontalSpeed;
    [SerializeField]
    [Range(0, 1)]
    private float verticalSpeed;
    [SerializeField]
    [Range(0, 1)]
    private float _speed;
    [SerializeField]
    [Range(0, 1)]
    private float _Yspeed;



    private float h;
    private float v;
    private Vector3 tp;

    private void Awake()
    {
        tp = new Vector3();
        Cursor.visible = false;
    }

    private void Update()
    {
        h += horizontalSpeed * Input.GetAxis("Mouse X");
        v += verticalSpeed * Input.GetAxis("Mouse Y");

        v = Mathf.Clamp(v, -85, 85);

        transform.rotation = Quaternion.Euler(-v, h, 0f);
        if (Input.GetKey(KeyCode.A)) transform.position -= transform.right * _speed;
        if (Input.GetKey(KeyCode.S)) transform.position -= transform.forward * _speed;
        if (Input.GetKey(KeyCode.D)) transform.position += transform.right * _speed;
        if (Input.GetKey(KeyCode.W)) transform.position += transform.forward * _speed;
        if (Input.GetKey(KeyCode.Mouse0)) transform.position += transform.up * _speed;
        if (Input.GetKey(KeyCode.Mouse1)) transform.position -= transform.up * _speed;

        tp = Vector3.zero;
    }

}