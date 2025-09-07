using UnityEngine;
using System.Collections.Generic;

public class Player_Animation : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public List<Sprite> sprites;
    Camera main;
    float angle;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        main = Camera.main;
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
    void Update()
    {
        angle = Player_Method.GetMouseToRotation(transform).z - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    public void UpdateBow(float ratio)
    {
        int idx = Mathf.FloorToInt(ratio * (sprites.Count));
        spriteRenderer.sprite = sprites[idx];
    }
}
