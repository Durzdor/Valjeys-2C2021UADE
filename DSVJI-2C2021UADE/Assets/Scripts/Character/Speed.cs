using System;
using System.Collections;
using UnityEngine;

public class Speed : MonoBehaviour
{
    private bool groundSpeedBuff;
    private bool jumpForceBuff;
    private bool sprintMultBuff;
    private Coroutine groundSpeedBuffTimer;
    private Coroutine jumpForceBuffTimer;
    private Coroutine sprintMultBuffTimer;

    [SerializeField] private float baseGroundSpeed = 6f;
    [SerializeField] private float baseJumpForce = 10f;
    [SerializeField] private float baseSprintMult = 2f;

    public float CurrentGroundSpeed { get; private set; }
    public float BaseGroundSpeed => baseGroundSpeed;
    public float CurrentJumpForce { get; private set; }
    public float BaseJumpForce => baseJumpForce;
    public float CurrentSprintMult { get; private set; }
    public float BaseSprintMult => baseSprintMult;

    public event Action OnGroundSpeedChange;
    public event Action OnJumpForceChange;
    public event Action OnSprintMultChange;

    private void Awake()
    {
        CurrentGroundSpeed = baseGroundSpeed;
        CurrentJumpForce = baseJumpForce;
        CurrentSprintMult = baseSprintMult;
    }

    public void ChangeGroundSpeed(float newGroundSpeed, float buffDuration)
    {
        if (groundSpeedBuff)
        {
            StopCoroutine(groundSpeedBuffTimer);
        }
        
        CurrentGroundSpeed = newGroundSpeed;
        groundSpeedBuffTimer = StartCoroutine(GroundBuffTimer(buffDuration));
    }

    public void ChangeJumpForce(float newJumpForce, float buffDuration)
    {
        if (jumpForceBuff)
        {
            StopCoroutine(jumpForceBuffTimer);
        }
        
        CurrentJumpForce = newJumpForce;
        jumpForceBuffTimer = StartCoroutine(JumpBuffTimer(buffDuration));
    }

    public void ChangeSprintMult(float newSprintMult, float buffDuration)
    {
        if (sprintMultBuff)
        {
            StopCoroutine(sprintMultBuffTimer);
        }

        CurrentSprintMult = newSprintMult;
        sprintMultBuffTimer = StartCoroutine(SprintBuffTimer(buffDuration));
    }

    private IEnumerator GroundBuffTimer(float buffDuration)
    {
        groundSpeedBuff = true;

        while (buffDuration > 0)
        {
            buffDuration -= Time.deltaTime;
            yield return null;
        }

        groundSpeedBuff = false;
        CurrentGroundSpeed = baseGroundSpeed;
    }

    private IEnumerator JumpBuffTimer(float buffDuration)
    {
        jumpForceBuff = true;

        while (buffDuration > 0)
        {
            buffDuration -= Time.deltaTime;
            yield return null;
        }

        jumpForceBuff = false;
        CurrentJumpForce = baseJumpForce;
    }

    private IEnumerator SprintBuffTimer(float buffDuration)
    {
        sprintMultBuff = true;

        while (buffDuration > 0)
        {
            buffDuration -= Time.deltaTime;
            yield return null;
        }

        sprintMultBuff = false;
        CurrentSprintMult = baseSprintMult;
    }
}