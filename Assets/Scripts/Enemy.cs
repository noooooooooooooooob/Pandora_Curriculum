using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    EnemyState currentState;
    NavMeshAgent agent;

    [Header("Move/Target")]
    public float speed = 3f;
    public Transform target;

    [Header("Attack Ranges")]
    public float[] attackRange;

    [Header("Timers")]
    public float exhaustedTime;
    public float chaseAttackDelay = 3f;
    public float attack3Duration = 2f;

    [Header("Pooling")]
    public GameObject attack1GameObject;
    public GameObject attack2GameObject;
    public GameObject attack3Prefab;
    public GameObject attack4GameObject;
    public GameObject attack5GameObject;

    float distance;
    float chaseStartTime;
    Coroutine attackRoutine;

    readonly List<GameObject> attack3Pool = new();

    void Start()
    {
        currentState = EnemyState.Chase;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;

        MakePool();
        EnterChase();
        StartCoroutine(StateLoop());
    }

    void Update()
    {
        if (currentState == EnemyState.Chase)
        {
            if (target != null)
            {
                agent.SetDestination(target.position);
                distance = Vector2.Distance(transform.position, target.position);
                if (distance <= attackRange[0])
                {
                    SwitchToAttack();
                    return;
                }
                if (Time.time - chaseStartTime >= chaseAttackDelay)
                {
                    SwitchToAttack();
                    return;
                }
            }
        }
    }

    IEnumerator StateLoop()
    {
        while (true)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    yield return new WaitForSeconds(exhaustedTime);
                    EnterChase();
                    break;
                case EnemyState.Chase:
                    break;
                case EnemyState.Attack:
                    if (attackRoutine == null)
                    {
                        if (distance <= attackRange[0])
                        {
                            attackRoutine = StartCoroutine(Attack1Coroutine());
                            yield return attackRoutine;
                            attackRoutine = null;
                        }
                        else
                        {
                            int n = Random.Range(0,4);
                            switch (n)
                            {
                                case 0:
                                    attackRoutine = StartCoroutine(Attack2Coroutine());
                                    yield return attackRoutine;
                                    attackRoutine = null;
                                    break;
                                case 1:
                                    attackRoutine = StartCoroutine(Attack3Coroutine());
                                    yield return attackRoutine;
                                    attackRoutine = null;
                                    break;
                                case 2:
                                    attackRoutine = StartCoroutine(Attack4Coroutine());
                                    yield return attackRoutine;
                                    attackRoutine = null;
                                    break;
                                case 3:
                                    attackRoutine = StartCoroutine(Attack5Coroutine());
                                    yield return attackRoutine;
                                    attackRoutine = null;
                                    break;
                                default:
                                    break;
                            }
                        }

                        currentState = EnemyState.Idle;
                    }
                    break;
            }

            yield return null;
        }
    }


    void EnterChase()
    {
        currentState = EnemyState.Chase;
        chaseStartTime = Time.time;
        if (agent != null && target != null)
        {
            agent.isStopped = false;
            agent.ResetPath();
            agent.SetDestination(target.position);
        }
    }

    void SwitchToAttack()
    {
        if (currentState == EnemyState.Attack) return; // 중복 전이 방지
        currentState = EnemyState.Attack;
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    void MakePool()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject obj = Instantiate(attack3Prefab, target.position, Quaternion.identity);
            obj.SetActive(false);
            attack3Pool.Add(obj);
        }
    }

    GameObject GetAttack3()
    {
        foreach (var obj in attack3Pool)
        {
            if (!obj.activeInHierarchy) return obj;
        }
        GameObject newObj = Instantiate(attack3Prefab, target.position, Quaternion.identity);
        newObj.SetActive(false);
        attack3Pool.Add(newObj);
        return newObj;
    }

    IEnumerator Attack3Coroutine()
    {
        float timer = 0f;
        float spawnDelay = 0.5f;
        const float shrink = 0.95f;

        while (timer < attack3Duration)
        {
            if (target == null) break;

            GameObject obj = GetAttack3();
            obj.transform.position = target.position;
            obj.SetActive(true);

            yield return new WaitForSeconds(spawnDelay);
            timer += spawnDelay;
            spawnDelay *= shrink;
        }
    }
    IEnumerator Attack1Coroutine()
    {
        attack1GameObject.SetActive(true);

        yield return new WaitUntil(() =>
        attack1GameObject == null
        || !attack1GameObject.activeInHierarchy
    );
    }
    IEnumerator Attack2Coroutine()
    {
        attack2GameObject.SetActive(true);
        yield return new WaitUntil(() =>
        attack2GameObject == null
        || !attack2GameObject.activeInHierarchy
    );
    }
    IEnumerator Attack4Coroutine()
    {
        attack4GameObject.SetActive(true);
        yield return new WaitUntil(() =>
        attack4GameObject == null
        || !attack4GameObject.activeInHierarchy
    );
    }
    IEnumerator Attack5Coroutine()
    {
        attack5GameObject.SetActive(true);
        yield return new WaitUntil(() =>
        attack5GameObject == null
        || !attack5GameObject.activeInHierarchy
    );
    }
}
