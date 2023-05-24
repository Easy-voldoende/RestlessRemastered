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
    private bool isRoaming;
    private bool isIdle;
    private bool isChasing;
    public float speed;
    public float roamRadius = 30.0f;
    public float minRoamTime = 1.0f;
    public float maxRoamTime = 5.0f;    
    public float angle;
    public float angleToPlayer;
    public bool inAngle;
    public Vector3 playerPos;
    public GameObject eye;
    private Animator anim;
    public GameObject playerObj;
    public GameObject cameraObj;
    public Transform myPos;
    public Transform enemyPos;
    public bool sceneStarted;
    
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
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(state.ToString());
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

                if (Vector3.Distance(myPos.position, target) < 2.5f)
                {
                    target = RandomNavSphere(transform.position, roamRadius, -1);
                    agent.SetDestination(target);
                }

                


                break;
            case EnemyState.Chasing:

                target = player.position;
                agent.SetDestination(target);
                if (Vector3.Distance(myPos.position, player.position) < 2f)
                {
                    Debug.Log("You died");
                    if(sceneStarted == false)
                    {
                        StartCoroutine(nameof(DeathScene));
                        sceneStarted = false;
                    }
                }                

                break;
            case EnemyState.Idle:



                break;
        }

        playerPos = player.position;
    }
    public IEnumerator DeathScene()
    {
        cameraObj.GetComponent<CustomizableCamera>().died = true;
        playerObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        playerObj.gameObject.transform.position = myPos.position;
        playerObj.gameObject.transform.rotation = myPos.rotation;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = enemyPos.position;
        transform.rotation = enemyPos.rotation;
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        nav.acceleration = 100000;
        nav.speed = 0;
        player.GetComponent<Animator>().SetTrigger("Died");
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("Died");
        
        

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
    public void Detect()
    {
        if (Vector3.Distance(transform.position, player.position) < 4)
        {
            state = EnemyState.Chasing;
        }

        Vector3 targetDir = playerPos - transform.position;
        angleToPlayer = Vector3.Angle(targetDir, transform.forward);

        if (angleToPlayer <= detectionAngle && Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            if (Physics.Linecast(transform.position, player.transform.position, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Looking at player");
                    state = EnemyState.Chasing;
                }
                else
                {
                    Debug.Log("Not looking at player");
                    state = EnemyState.Roaming;
                }
            }
        }
        Debug.DrawLine(eye.transform.position, player.transform.position, Color.red);
    }
    void OnDrawGizmosSelected()
    {
        float totalFOV = detectionAngle;
        float rayRange = detectionRange;
        float halfFOV = totalFOV / 2.0f;
        
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);       
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);

        
    }
}
