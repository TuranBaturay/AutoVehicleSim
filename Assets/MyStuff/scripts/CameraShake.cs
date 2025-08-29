using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float shake_duration = 0;
    private float elapsed_time = 0;
    private float amplitude;
    private float frequency;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake(float duration, float amplitude, float frequency)
    {
        this.amplitude = amplitude;
        this.frequency = frequency;
        shake_duration = duration;
        elapsed_time = 0;
    }

    void Update()
    {
        // Trigger shake with the space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shake(0.5f, 0.2f, 20f); // Example values: duration, amplitude, frequency
        }
        
        if (elapsed_time < shake_duration)
        {
            elapsed_time += Time.deltaTime;

            // Calculate shake offset
            float shakeAmount = amplitude * Mathf.Sin(frequency * elapsed_time * 2 * Mathf.PI);
            Vector3 offset = Random.insideUnitSphere * shakeAmount;

            transform.localPosition = originalPosition + offset;
        }
        else
        {
            // Reset position after shake
            transform.localPosition = originalPosition;
        }
    }
}