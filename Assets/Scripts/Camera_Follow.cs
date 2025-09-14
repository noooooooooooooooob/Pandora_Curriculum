using UnityEngine;
using System.Collections;

public class Camera_Follow : MonoBehaviour
{
    bool isFollow = true;
    public Transform target;          // 따라갈 대상
    public float smoothTime = 0.2f;   // 부드럽게 이동하는 시간
    private Vector3 velocity;
    private Vector3 offset;
    private float originalSize = 6f;

    private void Start()
    {
        if (target == null) return;
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target == null) return;
        if (!isFollow) return;

        transform.position = target.position;
    }

    public void PlusCameraSize(float size)
    {
        Camera.main.orthographicSize = originalSize + size * 1.5f;
    }

    public void ResetCameraSize()
    {
        StartCoroutine(ResetSizeSmooth());
    }

    private IEnumerator ResetSizeSmooth()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        float startSize = Camera.main.orthographicSize;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Lerp(startSize, originalSize, elapsed / duration);
            yield return null;
        }

        Camera.main.orthographicSize = originalSize;
    }
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }
    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        isFollow = false;
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
        isFollow = true;
    }
}
