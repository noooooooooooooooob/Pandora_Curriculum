using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

public class Attack1 : MonoBehaviour
{
    public GameObject weapon;
    public SpriteRenderer fullSpriteRenderer;
    public GameObject collider;
    public float delay;
    public float damageTime = 0.5f;
    float startTime;
    bool isDamaging = false;
    public float maxScale;
    [SerializeField] String soundName;
    void OnEnable()
    {
        startTime = Time.time;
        fullSpriteRenderer.transform.localScale = new Vector3(0, 0, 1);
        isDamaging = false;
        collider.SetActive(false);
        AudioManager.Instance.PlaySound(soundName);
    }
    void Update()
    {
        if (isDamaging) return;
        float elapsed = Time.time - startTime;
        float alpha = Mathf.Clamp01(elapsed / delay);
        SetAlpha(alpha);
    }
    void SetAlpha(float alpha)
    {
        if (alpha >= 1f)
        {
            isDamaging = true;
            StartCoroutine(DisableAfterDamageTime());
            return;
        }
        fullSpriteRenderer.transform.localScale = new Vector3(alpha * maxScale, alpha * maxScale, 1);

        // 크기(스케일) 변화 없음
    }
    IEnumerator DisableAfterDamageTime()
    {
        isDamaging = true;
        gameObject.SetActive(true);
        transform.DOKill();
        transform.localRotation = Quaternion.identity;
        collider.SetActive(true);

        // 첫 물리 스텝 보장해서 겹침 히트 놓치지 않게
        yield return new WaitForFixedUpdate();

        // 회전은 콜라이더가 달린 트랜스폼 기준으로
        Transform rotTarget = collider ? collider.transform : weapon.transform;

        Tween t = rotTarget
            .DORotate(new Vector3(0, 0, 360f), damageTime, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutCirc)
            .SetLink(gameObject);

        yield return t.WaitForCompletion();
        yield return new WaitForSeconds(0.3f);

        collider.SetActive(false);
        isDamaging = false;
        gameObject.SetActive(false);
    }
}
