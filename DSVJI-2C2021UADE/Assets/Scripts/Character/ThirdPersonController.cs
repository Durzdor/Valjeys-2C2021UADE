using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    private Character _character;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _jumpVelocity = Vector3.zero;
    private bool _isSprinting;
    private float _gravitySpeed = 20.0f;

    #region SerializedFields

#pragma warning disable 649
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
    
    public void FixedUpdate()
    {
        CoyoteTime();
        var h = _character.Input.HorizontalAxis;
        var v = _character.Input.VerticalAxis;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, _character.Camera.CameraYaw, 0),
            Time.deltaTime * rotationLerpSpeed); // Face camera
        
        _moveDirection = new Vector3(h, 0, v).normalized; // Strafe
        _moveDirection = transform.TransformDirection(_moveDirection);
        
        if (GroundedBonusTime)
        {
            _moveDirection *= moveDirectionSpeed; // Si esto se pone afuera de grounded, no pierde vel en el aire
            if (_character.Input.GetChangeSpeedInput && _character.Input.IsInputMoving)
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