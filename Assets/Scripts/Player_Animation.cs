using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Roll(float time)
    {
        spriteRenderer.color = Color.red;
        Invoke("ResetColor", time);
    }
    void ResetColor()
    {
        spriteRenderer.color = Color.white;
    }
}
