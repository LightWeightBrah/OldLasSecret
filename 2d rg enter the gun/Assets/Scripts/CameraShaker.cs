using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker instance;
    public static bool isShaking = false;
    public static Vector3 cameraPos = Vector3.zero;

    private void Awake()
    {
        instance = this;
    }

    public static void Shake(float duration, float magnitude)
    {
        if (isShaking) return;
        isShaking = true;
        instance.StartCoroutine(instance.DoShake(duration, magnitude));
    }

    public static void ShakeStrong(float duration, float magnitude)
    {
        instance.StartCoroutine(instance.DoShakeStrong(duration, magnitude));
    }

    public IEnumerator DoShake(float duration, float magnitude)
    {
        cameraPos = transform.localPosition;
        Vector3 shakePos = Vector3.zero;

        float elapsed = 0.0f; //how much time passed since start shaking

        while (elapsed < duration)
        {
            shakePos.x = Random.Range(-1f, 1f) * magnitude;
            shakePos.y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = cameraPos + shakePos;

            elapsed += Time.deltaTime;
            yield return null; //before continue to next itaration of while loop wait unitl next frame
        }

        transform.localPosition = cameraPos;
        isShaking = false;
    }

    public IEnumerator DoShakeStrong(float duration, float magnitude)
    {
        if (!isShaking) cameraPos = transform.localPosition;
        Vector3 shakePos = Vector3.zero;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            shakePos.x = Random.Range(-1f, 1f) * magnitude;
            shakePos.y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = cameraPos + shakePos;

            elapsed += Time.deltaTime;
            yield return null; //before continue to next itaration of while loop wait unitl next frame
        }

        transform.localPosition = cameraPos;
    }

}