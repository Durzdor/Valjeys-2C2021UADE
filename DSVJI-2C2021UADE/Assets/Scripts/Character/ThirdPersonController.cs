using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    private Character character;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 jumpVelocity = Vector3.zero;
    private bool isInputMoving;
    private bool isSprinting;
    
    private float cameraPitch = 15f; // starting angle downwards
    private float cameraYaw = 0; // starting angle sideways
    private float cameraDistance = 5.0f;
    private bool lerpYaw = false;
    private bool lerpDistance = false;
    private float cameraPitchSpeed = 2.0f;
    private float cameraPitchMin = -10.0f;
    private float cameraPitchMax = 80.0f;
    private float cameraYawSpeed = 5.0f;
    private float cameraDistanceSpeed = 5.0f;
    private float cameraDistanceMin = 2.0f;
    private float cameraDistanceMax = 12.0f;
    private float turnSpeed = 3.0f;
    private float gravitySpeed = 20.0f;

    #region SerializedFields
#pragma warning disable 649
    [Header("References")][Space(2)]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform mainCamera;
    [Header("Velocities")][Space(2)]
    [SerializeField] private float moveDirectionSpeed = 6f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float sprintSpeed = 2f;
    [SerializeField] private float rotationLerpSpeed = 10f;
#pragma warning restore 649
    #endregion

    public event Action OnJump;
    public event Action OnSprint;

    public Vector3 MoveDirection => moveDirection;
    public bool IsInputMoving => isInputMoving;
    public bool IsSprinting => isSprinting;

    #region CoyoteJump

    private float jumpButtonGracePeriod = 0.1f;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    public bool GroundedBonusTime => Time.time - lastGroundedTime <= jumpButtonGracePeriod;
    public bool CanJump => Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod;

    private void CoyoteTime()
    {
        if (character.Controller.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (character.CharacterInput.GetJumpInput)
        {
            jumpButtonPressedTime = Time.time;
        }
    }

    #endregion
    
    private void Awake()
    {
        character = GetComponent<Character>();
    }
    
    public void LateUpdate()
    {
        // If mouse button down then allow user to look around
        if (character.CharacterInput.GetCameraLookAround|| character.CharacterInput.GetCameraPlayerControl)
        {
            cameraPitch += character.CharacterSettings.InvertMouseY ? Input.GetAxis("Mouse Y") : (Input.GetAxis("Mouse Y") * -1f) * cameraPitchSpeed;
            cameraPitch = Mathf.Clamp(cameraPitch, cameraPitchMin, cameraPitchMax);
            cameraYaw += character.CharacterSettings.InvertMouseX ? (Input.GetAxis("Mouse X") * -1f) : Input.GetAxis("Mouse X") * cameraYawSpeed;
            cameraYaw %= 360.0f;
            lerpYaw = false;
        }
        else
        {
            // If moving then make camera follow
            if (lerpYaw)
                cameraYaw = Mathf.LerpAngle(cameraYaw, cameraTarget.eulerAngles.y, 5.0f * Time.deltaTime);
        }

        // Zoom
        if (character.CharacterInput.ZoomAxis != 0)
        {
            cameraDistance -= character.CharacterInput.ZoomAxis * cameraDistanceSpeed;
            cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
            lerpDistance = false;
        }

        // Calculate camera position
        Vector3 newCameraPosition = cameraTarget.position + (Quaternion.Euler(cameraPitch, cameraYaw, 0) * Vector3.back * cameraDistance);

        // Does new position put us inside anything?
        RaycastHit hitInfo;
        if (Physics.Linecast(cameraTarget.position, newCameraPosition, out hitInfo))
        {
            newCameraPosition = hitInfo.point;
            lerpDistance = true;
        }
        else
        {
            if (lerpDistance)
            {
                float newCameraDistance = Mathf.Lerp(Vector3.Distance(cameraTarget.position, mainCamera.transform.position), cameraDistance, 5.0f * Time.deltaTime);
                newCameraPosition = cameraTarget.position + (Quaternion.Euler(cameraPitch, cameraYaw, 0) * Vector3.back * newCameraDistance);
            }
        }

        mainCamera.transform.position = newCameraPosition;
        mainCamera.transform.LookAt(cameraTarget.position);
    }
    
    public void FixedUpdate()
    {
        CoyoteTime();
        var h = character.CharacterInput.HorizontalAxis;
        var v = character.CharacterInput.VerticalAxis;
        var strafeAxis = character.CharacterInput.StrafeAxis;

        if (h != 0 || v != 0)
        {
            isInputMoving = true;
        }
        else
        {
            isInputMoving = false;
        }
        // Have camera follow if moving
        if (!lerpYaw && (h != 0 || v != 0) || strafeAxis != 0)
            lerpYaw = true;
        
        if (character.CharacterInput.GetCameraPlayerControl)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(0, cameraYaw, 0), Time.deltaTime * rotationLerpSpeed); // Face camera
        }
        else
        {
            if (character.CharacterSettings.AdStrafe)
            {
                transform.Rotate(0, strafeAxis * turnSpeed, 0); // Turn left/right
            }
            else
            {
                transform.Rotate(0, h * turnSpeed, 0); // Turn left/right
            }
            
        }
        
        if (character.CharacterInput.GetCameraPlayerControl || character.CharacterSettings.AdStrafe)
            moveDirection = new Vector3(h, 0, v).normalized; // Strafe
        else
            moveDirection = Vector3.forward * v; // Move forward/backward

        moveDirection = transform.TransformDirection(moveDirection);
        
        if (GroundedBonusTime) // character.Controller.isGrounded
        {
            moveDirection *= moveDirectionSpeed;
            if (character.CharacterInput.GetChangeSpeedInput && isInputMoving)
            {
                isSprinting = true;
                moveDirection = new Vector3(moveDirection.x * sprintSpeed, jumpVelocity.y,
                    moveDirection.z * sprintSpeed);
                OnSprint?.Invoke();
            }
            else
            {
                isSprinting = false;
                OnSprint?.Invoke();
            }
            if (CanJump) // character.CharacterInput.GetJumpInput
            {
                jumpButtonPressedTime = null;
                lastGroundedTime = null; // Coyote
                jumpVelocity.y = jumpSpeed;
                OnJump?.Invoke();
            } 
            else {
                jumpVelocity = Vector3.zero;
            }
        } 
        else
        {
            moveDirection *= moveDirectionSpeed;
            jumpVelocity.y -= gravitySpeed * Time.deltaTime;
        }
        character.Controller.Move((moveDirection + jumpVelocity) * Time.deltaTime);
    }
}