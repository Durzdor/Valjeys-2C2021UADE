using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [Header("Movement")][Space(5)]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float sprintSpeed = 4f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float jumpsAmount = 2f;
    [SerializeField] private float gravity = -9.81f;
    [Header("Grounded")][Space(5)]
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask groundedLayer;
    [SerializeField] private GameObject marker;
    [SerializeField] private float groundDistance = 0.1f;
#pragma warning restore 649

    #endregion
    
    #region CoyoteJump

    private float _jumpButtonGracePeriod = 0.1f;
    private float? _lastGroundedTime;
    private float? _jumpButtonPressedTime;

    public bool GroundedBonusTime => Time.time - _lastGroundedTime <= _jumpButtonGracePeriod;
    private bool CanJump => Time.time - _jumpButtonPressedTime <= _jumpButtonGracePeriod;
    
    #endregion
    
    private Character _character;
    private Vector3 _velocity;
    private GameObject _lastGroundedPosition;
    private float _remainingJumps;

    public bool IsGrounded { get; private set; }
    public bool IsSprinting { get; private set; }
    
    public event Action OnJump;
    public event Action OnLanding;
    
    private void Awake()
    {
        _character = GetComponent<Character>();
    }
    
    private void Update()
    {
        CeilingCheck();
        CoyoteTime();
        GroundCheck();
        HorizontalMovement();
        VerticalMovement();
        StuckAirborneCheck();
    }

    private void CeilingCheck()
    {
        if ((_character.Controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            _velocity.y = -2f;
        }
    }
    
    private void CoyoteTime()
    {
        if (_character.Controller.isGrounded)
            _lastGroundedTime = Time.time;

        if (_character.Input.GetJumpInput)
            _jumpButtonPressedTime = Time.time;
    }
    
    private void GroundCheck()
    {
        var wasGrounded = IsGrounded;
        IsGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundedLayer);

        if (!wasGrounded && IsGrounded)
        {
            OnLanding?.Invoke();
        }
        
        if (!IsGrounded || !(_velocity.y < 0)) return;
        MarkerUpdate();
        _character.Controller.slopeLimit = 45.0f;
        _remainingJumps = jumpsAmount;
        _velocity.y = -2f;
    }

    private void HorizontalMovement()
    {
        var characterTransform = transform;
        var move = characterTransform.right * _character.Input.HorizontalAxis + characterTransform.forward * _character.Input.VerticalAxis;
        
        SprintCheck();
        _character.Controller.Move(move.normalized * ((IsSprinting ? sprintSpeed : walkSpeed) * Time.deltaTime));
    }

    private void VerticalMovement()
    {
        JumpCheck();
        _velocity.y += gravity * Time.deltaTime;
        _character.Controller.Move(_velocity * Time.deltaTime);
    }

    private void StuckAirborneCheck()
    {
        if (!IsGrounded &&  _velocity.y < -100)
        {
            _character.Teleport(_lastGroundedPosition.transform);
        }
    }

    private void MarkerUpdate()
    {
        var characterTransform = transform;
        if (_lastGroundedPosition == null)
        {
            _lastGroundedPosition = Instantiate(marker,characterTransform.position,Quaternion.identity);
        }
        else
        {
            _lastGroundedPosition.transform.position = characterTransform.position;
            _lastGroundedPosition.transform.rotation = characterTransform.rotation;
        }
    }

    private void SprintCheck()
    {
        IsSprinting = _character.Input.GetChangeSpeedInput && IsGrounded;
    }
    
    private void JumpCheck()
    {
        if (!CanJump || !(_remainingJumps > 0)) return;
        if (!_character.Input.GetJumpInput) return;
        
        _jumpButtonPressedTime = null;
        _lastGroundedTime = null;
        
        _character.Controller.slopeLimit = 100.0f;
        _velocity.y = Mathf.Sqrt(jumpHeight * (gravity * -3f));
        _remainingJumps--;
        
        OnJump?.Invoke();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundChecker.position,groundDistance);
    }
}
