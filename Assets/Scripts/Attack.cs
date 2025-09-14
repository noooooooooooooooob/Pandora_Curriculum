using UnityEngine;
using System.Collections;
using System;

public class Attack : MonoBehaviour
{
    public SpriteRenderer fullSpriteRenderer;
    public Collider2D fullCollider;
    public GameObject damageEffect;
    public float delay;
    public float damageTime;
    float startTime;
    protected bool isDamaging = false;
    [SerializeField] String soundName;
    void OnEnable()
    {
        startTime = Time.time;
        fullCollider.enabled = false;
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
            fullCollider.enabled = true;
            isDamaging = true;
            if (damageEffect != null)
            {
                damageEffect.SetActive(true);
                AudioManager.Instance.PlaySound(soundName);
            }
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
        yield return new WaitForSeconds(damageTime);
        damageEffect.SetActive(false);
        fullCollider.enabled = false;
        isDamaging = false;
        gameObject.SetActive(false);
    }
}
