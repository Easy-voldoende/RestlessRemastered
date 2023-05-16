using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyPathfinding : MonoBehaviour
{
    public bool canRoam;
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
    public float angle;
    public float angleToPlayer;
    public float currentAngle;
    
    public float distanceToTarget;
    public LayerMask layerMask;
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
        Debug.Log(state.ToString());
        Detect();
        SwitchStates();
    }

    public void SwitchStates()
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

                


                break;
            case EnemyState.Chasing:

                target = player.position;
                agent.SetDestination(target);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, layerMask))
                {
                    transform.LookAt(player.position);
                }
                if (Vector3.Distance(myPos.position, player.position) < 2f)
                {
                    Debug.Log("You died");
                }                

                break;
            case EnemyState.Idle:



                break;
        }

        
    }
     
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }


    public float detectionRange;
    public float detectionAngle;
    public RaycastHit hit;
    private void Detect()
    {
        if (Vector3.Distance(transform.position, player.position) < 4)
        {

            state = EnemyState.Chasing;
        }

        Vector3 targetDir = target - transform.position;
        angleToPlayer = Vector3.Angle(targetDir, transform.forward);
        

        if (angleToPlayer < 45.0f)
        {
            if(Physics.Raycast(transform.position, player.position, out hit, detectionRange, layerMask))
            {
                lookingAtPlayer = true;
            }
            else
            {
                lookingAtPlayer = false;
            }
        }
        else
        {
            lookingAtPlayer = false;
        }

        if (angleToPlayer <= detectionAngle && lookingAtPlayer == true)
        {
            
            state = EnemyState.Chasing;
        }

        Debug.DrawRay(transform.position, transform.forward * detectionRange, Color.green);

    }

}
