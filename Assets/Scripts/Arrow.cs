using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform target;
    void Start()
    {
        go();
    }
    void go()
    {
        DOTween.To(() => transform.position, x => transform.position = x, target.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
