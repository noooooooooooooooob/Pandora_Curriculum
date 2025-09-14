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
    public float maxScale;
    [SerializeField] String soundName;
    void OnEnable()
    {
        startTime = Time.time;
        fullCollider.enabled = false;
        fullSpriteRenderer.transform.localScale = new Vector3(0, 0, 1);
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
            fullSpriteRenderer.transform.localScale = new Vector3(alpha, alpha, 1);
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
