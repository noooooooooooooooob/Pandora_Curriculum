using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Running_Effect : MonoBehaviour
{
    public GameObject effectPrefab;
    public List<Running_Effect_Offset> offsets = new List<Running_Effect_Offset>()  // 0 : up  시계방향
    {
        new Running_Effect_Offset(){ offset = new Vector3(0, 0.7f, 0), angle = new Vector3Int(0, 0, 270) },
        new Running_Effect_Offset(){ offset = new Vector3(0.8f, 0.5f, 0), angle = new Vector3Int(0, 0, 220) },
        new Running_Effect_Offset(){ offset = new Vector3(0.9f, 0, 0), angle = new Vector3Int(0, 180, 35) },
        new Running_Effect_Offset(){ offset = new Vector3(0.6f, -0.35f, 0), angle = new Vector3Int(0, 180, 0) },
        new Running_Effect_Offset(){ offset = new Vector3(0.25f, -0.75f, 0), angle = new Vector3Int(0, 180, 90) },
        new Running_Effect_Offset(){ offset = new Vector3(-0.55f, -0.45f, 0), angle = new Vector3Int(0, 0, 60) },
        new Running_Effect_Offset(){ offset = new Vector3(-0.7f, 0, 0), angle = new Vector3Int(0, 0, 0) },
        new Running_Effect_Offset(){ offset = new Vector3(-0.4f, 0.5f, 0), angle = new Vector3Int(0, 0, -25) }
    }; 
    float intervalTime = 0.3f;
    public bool isExisting = false;
    public void PlayEffect(int idx)
    {
        StartCoroutine(EffectCoroutine(idx));
    }
    IEnumerator EffectCoroutine(int idx)
    {
        if (isExisting) yield break;
        isExisting = true;
        GameObject effect = Instantiate(effectPrefab, transform.position + offsets[idx].offset, Quaternion.Euler(offsets[idx].angle));
        yield return new WaitForSeconds(intervalTime);
        isExisting = false;
    }
}

public class Running_Effect_Offset
{
    public Vector3 offset;
    public Vector3Int angle;
}