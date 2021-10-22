using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [Header("References")] [Space(2)] 
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject firstPersonCamera;
    [SerializeField] private LayerMask cameraCollisionLayers;

    [Header("Camera attributes")] [Space(5)] 
    [SerializeField] private float cameraPitch = 15f; // starting angle downwards
    [SerializeField] private float cameraYaw; // starting angle sideways
    [SerializeField] private float cameraDistance = 5.0f;
    [SerializeField] private float cameraPitchSpeed = 2.0f;
    [SerializeField] private float cameraPitchMin = -10.0f;
    [SerializeField] private float cameraPitchMax = 80.0f;
    [SerializeField] private float cameraYawSpeed = 5.0f;
    [SerializeField] private float cameraDistanceSpeed = 5.0f;
    [SerializeField] private float cameraDistanceMin = 0f;
    [SerializeField] private float cameraDistanceMax = 12.0f;
#pragma warning restore 649

    #endregion

    private Character _character;
    private bool _lerpYaw;
    private bool _lerpDistance;
    private Vector3 _cameraTargetPosition;
    private Vector3 _newCameraTargetPosition;
    private float _cameraDistanceFromTarget;
    private RaycastHit _raycastHit;
    public float CameraYaw => cameraYaw;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private void Update()
    {
        if (!_lerpYaw && (_character.Input.HorizontalAxis != 0 || _character.Input.VerticalAxis != 0))
            _lerpYaw = true;
    }

    public void LateUpdate()
    {
        // If mouse button down then allow user to look around
        cameraPitch += _character.Settings.InvertMouseY
            ? _character.Input.MouseYAxis
            : (_character.Input.MouseYAxis * -1f) * cameraPitchSpeed;
        cameraPitch = Mathf.Clamp(cameraPitch, cameraPitchMin, cameraPitchMax);
        cameraYaw += _character.Settings.InvertMouseX
            ? (_character.Input.MouseXAxis * -1f)
            : _character.Input.MouseXAxis * cameraYawSpeed;
        cameraYaw %= 360.0f;
        _lerpYaw = false;

        // If moving then make camera follow
        if (_lerpYaw)
            cameraYaw = Mathf.LerpAngle(cameraYaw, cameraTarget.eulerAngles.y, 5.0f * Time.deltaTime);

        // Zoom
        if (_character.Input.ZoomAxis != 0)
        {
            cameraDistance -= _character.Input.ZoomAxis * cameraDistanceSpeed;
            cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
            _lerpDistance = false;
        }

        _cameraTargetPosition = cameraTarget.position;
        _newCameraTargetPosition = _cameraTargetPosition +
                                   (Quaternion.Euler(cameraPitch, cameraYaw, 0) * Vector3.back * cameraDistance);

        _cameraDistanceFromTarget = Vector3.Distance(_cameraTargetPosition, mainCamera.transform.position);
        // Does new position put us inside anything?
        if (Physics.SphereCast(cameraTarget.position, 0.6f, (_newCameraTargetPosition - cameraTarget.position), out _raycastHit, cameraDistance, cameraCollisionLayers ))
        {
            var center = _raycastHit.point + 0.6f * _raycastHit.normal;
            _newCameraTargetPosition = center;
            _lerpDistance = true;
        }
        else
        {
            if (_lerpDistance)
            {
                var newCamDistance = Mathf.Lerp(_cameraDistanceFromTarget, cameraDistance, 5.0f * Time.deltaTime);
                _newCameraTargetPosition = _cameraTargetPosition +
                                           (Quaternion.Euler(cameraPitch, cameraYaw, 0) * Vector3.back * newCamDistance);
            }
        }
        if (Physics.Linecast(_cameraTargetPosition,_newCameraTargetPosition,cameraCollisionLayers))
        {
            mainCamera.SetActive(false);
            firstPersonCamera.SetActive(true);
        }
        else
        {
            mainCamera.SetActive(true);
            firstPersonCamera.SetActive(false);
        }
        mainCamera.transform.position = _newCameraTargetPosition;
        mainCamera.transform.LookAt(_cameraTargetPosition);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_cameraTargetPosition, _newCameraTargetPosition);
        Gizmos.DrawWireSphere(_raycastHit.point, 0.6f);
    }
}