using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    EnemyState currentState;
    NavMeshAgent agent;
    public Transform target;
    public float exhaustedTime;

    void Start()
    {
        currentState = EnemyState.Chase;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // Agent 가 Target 을 향해 이동할 때 방향을 회전할지
        agent.updateUpAxis = false; // 캐릭터의 이동을 평면으로 제한하기 위해
        StartCoroutine(CheckState());
    }
    void Update()
    {
        if (currentState == EnemyState.Chase)
        {
            agent.SetDestination(target.position);
            if (Vector2.Distance(transform.position, target.position) < 0.5f)
            {
                currentState = EnemyState.Attack;
            }
        }
    }
    IEnumerator CheckState()
    {
        while (true)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    yield return new WaitForSeconds(3.0f);
                    break;
                case EnemyState.Chase:
                    break;
                case EnemyState.Attack:
                    float range = Vector2.Distance(transform.position, target.position);
                    Attack(range);
                    break;
                case EnemyState.Exhausted:
                    Exhausted();
                    yield return new WaitForSeconds(exhaustedTime);
                    break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    void Attack(float range)
    {
        if (range < 1.5f)
        {
            Attack1();
        }
        else if (range < 5.0f)
        {
            Attack2();
        }
        else
        {
            Attack3();
        }
    }
    void Attack1()
    {

    }
    void Attack2()
    {

    }
    void Attack3()
    {

    }
    void Exhausted()
    {

    }
}
