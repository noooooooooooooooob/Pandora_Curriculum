using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Attack5 : MonoBehaviour
{
    public List<GameObject> attackList;
    public int repetitionTime = 3;
    public float term = 0.5f;
    float faster = 0.96f;
    public Transform targetPos;
    public float angleJitter = 5f; 
    public bool realignEachWave = true;

    float angle;

    void OnEnable()
    {
        StopAllCoroutines();
        foreach (var a in attackList)
            if (a) a.SetActive(false);

        SetAngle();
        StartCoroutine(StartPattern());
    }

    public void SetAngle()
    {
        if (targetPos == null) return;

        Vector2 dir = (Vector2)(targetPos.position - transform.position);
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float rd = Random.Range(-angleJitter, angleJitter);
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f + rd);
    }

    IEnumerator StartPattern()
    {
        float originalTerm = term;
        for (int i = 0; i < repetitionTime; i++)
        {
            term = originalTerm;
            if (realignEachWave) SetAngle();
            foreach (var attack in attackList)
            {
                if (!attack) continue;

                attack.SetActive(true);
                yield return new WaitForSeconds(term);
                term *= faster;
            }
            yield return new WaitUntil(AllInactive);
        }

        gameObject.SetActive(false);
    }

    bool AllInactive()
    {
        foreach (var attack in attackList)
        {
            if (attack && attack.activeInHierarchy)
                return false;
        }
        return true;
    }

    void OnDisable()
    {
        StopAllCoroutines();
        foreach (var attack in attackList)
            if (attack) attack.SetActive(false);
    }
}
