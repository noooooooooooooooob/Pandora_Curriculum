using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Attack5 : MonoBehaviour
{
    public List<GameObject> attackList;
    public int repetitionTime = 3;
    public float term = 0.5f;
    float faster = 0.95f;
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
        transform.rotation = Quaternion.Euler(0, 0, angle+ rd);
    }

    IEnumerator StartPattern()
{
    float currentTerm = term; // 웨이브별 간격

    for (int i = 0; i < repetitionTime; i++)
    {
        for(int j = 0; j < attackList.Count; j++)
        {
            if (!attackList[j]) attackList[j].SetActive(false);

            attackList[j].SetActive(true);
            yield return new WaitForSeconds(currentTerm);
        }
        currentTerm *= faster; // faster = 0.99f 등
    }
    yield return new WaitUntil(AllInactive);
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
