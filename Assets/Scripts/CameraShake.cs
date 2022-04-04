using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public Transform cam;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    public bool continuous = false;

    Vector3 originalPos;

    void Awake()
    {

    }

    void OnEnable()
    {
        originalPos = cam.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0 || continuous)
        {
            cam.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            cam.localPosition = originalPos;
        }
    }

    public void ShakeNormal()
    {
        shakeDuration = 0.5f;
    }

    public void FramePause()
    {
        StartCoroutine(Pause());
    }

    IEnumerator Pause()
    {
        Time.timeScale = 0f;
        yield return 0;
        yield return 0;
        yield return 0;
        Time.timeScale = 1f;
    }


}