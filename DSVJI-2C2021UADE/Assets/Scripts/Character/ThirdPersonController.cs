using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    private Character _character;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _jumpVelocity = Vector3.zero;
    private bool _isInputMoving;
    private bool _isSprinting;

    private float _cameraPitch = 15f; // starting angle downwards
    private float _cameraYaw; // starting angle sideways
    private float _cameraDistance = 5.0f;
    private bool _lerpYaw;
    private bool _lerpDistance;
    private float _cameraPitchSpeed = 2.0f;
    private float _cameraPitchMin = -10.0f;
    private float _cameraPitchMax = 80.0f;
    private float _cameraYawSpeed = 5.0f;
    private float _cameraDistanceSpeed = 5.0f;
    private float _cameraDistanceMin = 2.0f;
    private float _cameraDistanceMax = 12.0f;
    private float _gravitySpeed = 20.0f;

    #region SerializedFields

#pragma warning disable 649
    [Header("References")] [Space(2)] 
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private LayerMask cameraCollisionLayers;

    [Header("Velocities")] [Space(2)] 
    [SerializeField] private float moveDirectionSpeed = 6f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float sprintSpeed = 2f;
    [SerializeField] private float rotationLerpSpeed = 10f;
#pragma warning restore 649

    #endregion

    public event Action OnJump;
    public event Action OnSprint;

    public Vector3 MoveDirection => _moveDirection;
    public bool IsInputMoving => _isInputMoving;
    public bool IsSprinting => _isSprinting;

    #region CoyoteJump

    private float _jumpButtonGracePeriod = 0.1f;
    private float? _lastGroundedTime;
    private float? _jumpButtonPressedTime;

    public bool GroundedBonusTime => Time.time - _lastGroundedTime <= _jumpButtonGracePeriod;
    public bool CanJump => Time.time - _jumpButtonPressedTime <= _jumpButtonGracePeriod;

    private void CoyoteTime()
    {
        if (_character.Controller.isGrounded)
            _lastGroundedTime = Time.time;

        if (_character.Input.GetJumpInput)
            _jumpButtonPressedTime = Time.time;
    }

    #endregion

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void LateUpdate()
    {
        // If mouse button down then allow user to look around

        _cameraPitch += _character.Settings.InvertMouseY
            ? _character.Input.MouseYAxis
            : (_character.Input.MouseYAxis * -1f) * _cameraPitchSpeed;
        _cameraPitch = Mathf.Clamp(_cameraPitch, _cameraPitchMin, _cameraPitchMax);
        _cameraYaw += _character.Settings.InvertMouseX
            ? (_character.Input.MouseXAxis * -1f)
            : _character.Input.MouseXAxis * _cameraYawSpeed;
        _cameraYaw %= 360.0f;
        _lerpYaw = false;

        // If moving then make camera follow
        if (_lerpYaw)
            _cameraYaw = Mathf.LerpAngle(_cameraYaw, cameraTarget.eulerAngles.y, 5.0f * Time.deltaTime);

        // Zoom
        if (_character.Input.ZoomAxis != 0)
        {
            _cameraDistance -= _character.Input.ZoomAxis * _cameraDistanceSpeed;
            _cameraDistance = Mathf.Clamp(_cameraDistance, _cameraDistanceMin, _cameraDistanceMax);
            _lerpDistance = false;
        }

        // Calculate camera position
        Vector3 newCameraPosition = cameraTarget.position +
                                    (Quaternion.Euler(_cameraPitch, _cameraYaw, 0) * Vector3.back * _cameraDistance);

        // Does new position put us inside anything?
        if (Physics.Linecast(cameraTarget.position, newCameraPosition, out var hitInfo, cameraCollisionLayers))
        {
            newCameraPosition = hitInfo.point;
            _lerpDistance = true;
        }
        else
        {
            if (_lerpDistance)
            {
                float newCameraDistance =
                    Mathf.Lerp(Vector3.Distance(cameraTarget.position, mainCamera.transform.position), _cameraDistance,
                        5.0f * Time.deltaTime);
                newCameraPosition = cameraTarget.position +
                                    (Quaternion.Euler(_cameraPitch, _cameraYaw, 0) * Vector3.back * newCameraDistance);
            }
        }

        mainCamera.transform.position = newCameraPosition;
        mainCamera.transform.LookAt(cameraTarget.position);
    }

    public void FixedUpdate()
    {
        CoyoteTime();
        var h = _character.Input.HorizontalAxis;
        var v = _character.Input.VerticalAxis;

        if (h != 0 || v != 0)
            _isInputMoving = true;
        else
            _isInputMoving = false;

        // Have camera follow if moving
        if (!_lerpYaw && (h != 0 || v != 0))
            _lerpYaw = true;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, _cameraYaw, 0),
            Time.deltaTime * rotationLerpSpeed); // Face camera
        
        _moveDirection = new Vector3(h, 0, v).normalized; // Strafe
        _moveDirection = transform.TransformDirection(_moveDirection);
        
        if (GroundedBonusTime) // character.Controller.isGrounded
        {
            _moveDirection *= moveDirectionSpeed; // Si esto se pone afuera de grounded, no pierde vel en el aire
            if (_character.Input.GetChangeSpeedInput && _isInputMoving)
            {
                _isSprinting = true;
                _moveDirection = new Vector3(_moveDirection.x * sprintSpeed, _jumpVelocity.y,
                    _moveDirection.z * sprintSpeed);
                OnSprint?.Invoke();
            }
            else
            {
                _isSprinting = false;
                OnSprint?.Invoke();
            }

            if (CanJump) // character.CharacterInput.GetJumpInput
            {
                _jumpButtonPressedTime = null;
                _lastGroundedTime = null; // Coyote
                _jumpVelocity.y = jumpSpeed;
                OnJump?.Invoke();
            }
            else
            {
                _jumpVelocity = Vector3.zero;
            }
        }
        else
        {
            _moveDirection *= moveDirectionSpeed;
            _jumpVelocity.y -= _gravitySpeed * Time.deltaTime;
        }

        _character.Controller.Move((_moveDirection + _jumpVelocity) * Time.deltaTime);
    }
}