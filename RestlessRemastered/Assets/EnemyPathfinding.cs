using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyPathfinding : MonoBehaviour
{
    private NavMeshAgent agent;
    public Vector3 target;
    public Transform player;
    public float damage;
    public bool isRoaming;
    public bool isIdle;
    public bool isChasing;
    public float speed;
    public float roamRadius = 30.0f;
    public float minRoamTime = 1.0f;
    public float maxRoamTime = 5.0f;    
    private float nextRoamTime;
    private float angle = 45f;
    public float currentAngle;
    
    public float distanceToTarget;
    public enum EnemyState
    {
        Roaming,
        Chasing,
        Idle,

    }
    public EnemyState state;
    public bool lookingAtPlayer;
    

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target);
        Transform myPos = gameObject.transform;
        switch (state)
        {
            case EnemyState.Roaming:

                if (Vector3.Distance(myPos.position, target) < 2f)
                {
                    target = RandomNavSphere(transform.position, roamRadius, -1);
                    agent.SetDestination(target);
                }

                if (lookingAtPlayer == true&& Vector3.Distance(myPos.position, player.position)<20)
                {
                    state = EnemyState.Chasing;
                }


                break;
            case EnemyState.Chasing:

                target = player.position;
                agent.SetDestination(target);

                if(Vector3.Distance(myPos.position, player.position) < 2f)
                {
                    Debug.Log("You died");
                }
                if (lookingAtPlayer == true && Vector3.Distance(myPos.position, player.position) > 20)
                {
                    target = RandomNavSphere(transform.position, roamRadius, -1);
                    agent.SetDestination(target);
                    state = EnemyState.Roaming;
                }
                if (lookingAtPlayer == false && Vector3.Distance(myPos.position, player.position) > 20)
                {
                    target = RandomNavSphere(transform.position, roamRadius, -1);
                    agent.SetDestination(target);
                    state = EnemyState.Roaming;
                }

                break;
            case EnemyState.Idle:



                break;
        }

        Vector3 directionToObject = player.transform.position - transform.position;
        Vector3 forward = transform.forward;
        float dot = Vector3.Dot(directionToObject.normalized, forward.normalized);
        float angleBetween = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (angleBetween <= angle)
        {
            lookingAtPlayer = true;
        }
        else
        {
            lookingAtPlayer = false;
        }
        currentAngle = angleBetween;
    }
     
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
