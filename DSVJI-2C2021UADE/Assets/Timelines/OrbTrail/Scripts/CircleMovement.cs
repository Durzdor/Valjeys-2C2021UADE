using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    [SerializeField] private float maxAmplitude = 1f;
    [SerializeField] private float frequency = 1f;
    [SerializeField] private float animationDuration = 3f;
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float phase = 0f;

    public float xPos;
    public float zPos;

    private float _currentAmplitude;

    private void Start()
    {
        _currentAmplitude = maxAmplitude;
    }

    void Update()
    {
        xPos = _currentAmplitude * Mathf.Sin(Time.time * frequency + phase);
        zPos = _currentAmplitude * -Mathf.Cos(Time.time * frequency + phase);
        transform.position = new Vector3(xPos, transform.position.y, zPos);
    }

    [ContextMenu("Play Animation Forward")]
    private void AnimateForward()
    {
        StartCoroutine(AnimationForwardCoroutine());
    }

    [ContextMenu("Play Animation Backward")]
    private void AnimateBackward()
    {
        StartCoroutine(AnimationBackwardCoroutine());
    }

    private IEnumerator AnimationForwardCoroutine()
    {
        var coroutineTimer = 0f;

        while (coroutineTimer < animationDuration)
        {
            coroutineTimer = Mathf.Min(coroutineTimer + Time.deltaTime, animationDuration);
            var percentage = animationCurve.Evaluate((animationDuration - coroutineTimer) / animationDuration); //1-0
            _currentAmplitude = maxAmplitude * percentage;
            yield return null;
        }

        Debug.Log("Finish");
    }

    private IEnumerator AnimationBackwardCoroutine()
    {
        var coroutineTimer = 0f;

        while (coroutineTimer < animationDuration)
        {
            coroutineTimer = Mathf.Min(coroutineTimer + Time.deltaTime, animationDuration);
            var percentage = animationCurve.Evaluate(coroutineTimer / animationDuration); //0-1
            Debug.Log($"t: {percentage}");
            _currentAmplitude = maxAmplitude * percentage;
            yield return null;
        }

        Debug.Log("Finish");
    }
}
