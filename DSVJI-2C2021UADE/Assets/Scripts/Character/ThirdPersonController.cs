using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    
    private float cameraPitch = 40.0f;
    private float cameraYaw = 0;
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
    [SerializeField][Tooltip("Multipler of moveDirectionSpeed")] private float sprintSpeed = 2f;

    public event Action OnJump;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void LateUpdate()
    {
        // If mouse button down then allow user to look around
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            cameraPitch += Input.GetAxis("Mouse Y") * cameraPitchSpeed;
            cameraPitch = Mathf.Clamp(cameraPitch, cameraPitchMin, cameraPitchMax);
            cameraYaw += Input.GetAxis("Mouse X") * cameraYawSpeed;
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

        // Have camera follow if moving
        if (!lerpYaw && (h != 0 || v != 0))
            lerpYaw = true;

        if (Input.GetMouseButton(1))
            transform.rotation = Quaternion.Euler(0, cameraYaw, 0); // Face camera
        else
            transform.Rotate(0, h * turnSpeed, 0); // Turn left/right

        // Only allow user control when on ground
        if (controller.isGrounded)
        {
            if (Input.GetMouseButton(1))
                moveDirection = new Vector3(h, 0, v).normalized; // Strafe
            else
                moveDirection = Vector3.forward * v; // Move forward/backward

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveDirectionSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                 moveDirection *= sprintSpeed;
            }
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
                OnJump?.Invoke();
            }
        }

        moveDirection.y -= gravitySpeed * Time.deltaTime; // Apply gravity
        controller.Move(moveDirection * Time.deltaTime);
    }
}