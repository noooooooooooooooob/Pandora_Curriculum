using UnityEngine;
using DG.Tweening;

public class Camera_Follow : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed = 1.0f;
    private Vector3 offset;
    private Vector3 targetPos;
    float originalSize = 6f;

    private void Start()
    {
        if (target == null) return;

        offset = transform.position - target.position;
    }

    private void Update()
    {
        if (target == null) return;

        targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
    public void PlusCameraSize(float size)
    {
        Camera.main.orthographicSize = originalSize + size * 2;
    }
    public void ResetCameraSize()
    {
        DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, originalSize, 0.5f);
    }
}
