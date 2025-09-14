using UnityEngine;
using System.Collections;
using DG.Tweening;
using Unity.Profiling;
using Unity.VisualScripting;
using System;

public class Attack4 : MonoBehaviour
{
    public SpriteRenderer fullSpriteRenderer;
    public GameObject damageEffect;
    public float delay;
    public float damageTime;
    bool isDamaging = false;
    float startTime;
    [SerializeField] String soundName;
    void OnEnable()
    {
        startTime = Time.time;
        Color c = fullSpriteRenderer.color;
        c.a = 0f;
        fullSpriteRenderer.color = c;
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
            damageEffect.SetActive(true);
            StartCoroutine(DisableAfterDamageTime());
            return;
        }

        if (fullSpriteRenderer != null)
        {
            Color c = fullSpriteRenderer.color;
            c.a = alpha;
            fullSpriteRenderer.color = c;
        }
    }
    IEnumerator DisableAfterDamageTime()
    {
        if (damageEffect)
        {
            AudioManager.Instance.PlaySound(soundName);
            float startLocalY = damageEffect.transform.localPosition.y;

            Tween t = damageEffect.transform
                .DOLocalMoveY(startLocalY + 3.27f, damageTime)
                .SetEase(Ease.InOutQuint)
                .SetLink(damageEffect, LinkBehaviour.KillOnDisable | LinkBehaviour.KillOnDestroy);

            yield return t.WaitForCompletion();
            damageEffect.transform.localPosition = new Vector3(0, startLocalY, 0);
            damageEffect.SetActive(false);
        }
        yield return new WaitForSeconds(0.2f);
        isDamaging = false;
        gameObject.SetActive(false);
    }
}
