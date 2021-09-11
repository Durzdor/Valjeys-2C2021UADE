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
    private bool canControlMovement = true;

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
    
    [Header("References")][Space(2)]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform mainCamera;
    [Header("Velocities")][Space(2)]
    [SerializeField] private float moveDirectionSpeed = 6f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float sprintSpeed = 2f;
    [SerializeField] private float rotationLerpSpeed = 10f;

    public event Action OnJump;
    public event Action OnSprint;

    public Vector3 MoveDirection => moveDirection;
    public bool IsInputMoving => isInputMoving;
    public bool IsSprinting => isSprinting;

    private void Awake()
    {
        character = GetComponent<Character>();
        character.OnCharacterMoveLock += LockMovement;
    }

    private void LockMovement()
    {
        canControlMovement = !canControlMovement;
    }

    public void LateUpdate()
    {
        // If mouse button down then allow user to look around
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
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
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * cameraDistanceSpeed;
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
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        var strafeAxis = Input.GetAxis("SecondaryHorizontal");

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
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

        if (Input.GetMouseButton(1))
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

        if (!canControlMovement) return;

        if (Input.GetMouseButton(1) || character.CharacterSettings.AdStrafe)
            moveDirection = new Vector3(h, 0, v).normalized; // Strafe
        else
            moveDirection = Vector3.forward * v; // Move forward/backward

        moveDirection = transform.TransformDirection(moveDirection);
        if (character.Controller.isGrounded) 
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
            if (character.CharacterInput.GetJumpInput) 
            {
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