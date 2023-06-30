using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.Animations;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Newtonsoft.Json.Serialization;

public class EnemyPathfinding : MonoBehaviour
{
    public bool canRoam;
    public AudioSource jumpscare;
    public float rotationAdjustment;
    public GameObject[] eyes;
    public NavMeshAgent agent;
    public Vector3 target;
    public Transform lastPlayerPos;
    public Transform player;
    public Animator cameraAnim;
    public float damage;
    private float speed;
    public Animator Ui;
    public float runningSpeed;
    public float walkingSpeed;
    public float roamRadius = 50.0f;
    public float minRoamTime = 1.0f;
    public float maxRoamTime = 5.0f;    
    public float angle;
    public float i;
    public bool chasing;
    public float angleToPlayer;
    public bool inAngle;
    public Vector3 playerPos;
    public GameObject eye;
    public Animator anim;
    public GameObject playerObj;
    public GameObject cameraObj;
    public Transform myPos;
    public Transform enemyPos;
    public bool sceneStarted;
    public int shadowState;
    public float distanceToTarget;
    public LayerMask layerMask;
    public Vector3 origin;
    public int lookState;
    public GameObject focus;
    public bool manual;
    public AudioMixer audioMixer;
    public GameObject shadowPrefab;
    public bool test = false;
    public SliderSpin[] spins;
    public bool isMain;
    public int died;
    public bool canAnimateEye;
    public enum EnemyState
    {
        Roaming,
        Chasing,
        Idle,
        LookingAround,

    }
    public EnemyState state;
    public bool lookingAtPlayer;
    
    public IEnumerator NewTarget()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        target = RandomNavSphere(origin, roamRadius, -1);
        agent.SetDestination(target);
        StartCoroutine(SetNew());

    }
    public IEnumerator SetNew()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        StartCoroutine(NewTarget());

    }
    private void Start()
    {
        lookState = -1;
        died = PlayerPrefs.GetInt("Died");
        StartCoroutine(NewTarget());
        shadowState = 0;
        chasing = false;
        state = EnemyState.Roaming;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = transform.position;
        origin = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        Detect();
        SwitchStates(); 
        CheckForFootstep();

        if (Input.GetKeyDown(KeyCode.T))
        {
            GetComponent<EnemyPathfinding>().enabled = false;
            lookState = 0;
        }
        if(canAnimateEye == true)
        {
            Ui.SetInteger("LookState", lookState);
        }
    }

    public void SwitchStates()
    {
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        distanceToTarget = Vector3.Distance(transform.position, target);
        Transform myPos = gameObject.transform;
        switch (state)
        {
            case EnemyState.Roaming:                
                if(chasing == true)
                {
                    lookState = 1;
                    foreach (GameObject eyes in eyes)
                    {
                        eyes.GetComponent<LensFlareComponentSRP>().enabled = false;
                    }
                    shadowState = 2;
                    anim.SetInteger("State", shadowState);
                    nav.speed = 0;
                    i -= 1 * Time.deltaTime;
                    if (i < 0)
                    {                        
                        chasing = false;
                    }
                }
                else
                {
                    lookState = 0;

                    nav.speed = walkingSpeed;
                    shadowState = 0;
                    anim.SetInteger("State", shadowState);
                    if (Vector3.Distance(myPos.position, target) < 4f)
                    {
                        target = RandomNavSphere(origin, roamRadius, -1);
                        agent.SetDestination(target);
                    }
                }


                break;
            case EnemyState.Chasing:
                lookState = 1;
                nav.speed = runningSpeed;
                shadowState = 1;
                anim.SetInteger("State", shadowState);
                target = player.position;
                if (agent.enabled == true)
                {
                    agent.SetDestination(target);
                }
                if (Vector3.Distance(myPos.position, player.position) < 4f)
                {
                    Debug.Log("You died");
                    if(sceneStarted == false)
                    {
                        StartCoroutine(nameof(DeathScene));
                        sceneStarted = false;
                        
                    }
                }                

                break;
            case EnemyState.LookingAround:
                


                break;
        }

        playerPos = player.position;
    }
    public IEnumerator DeathScene()
    {
        if(isMain == true)
        {
            PlayerPrefs.SetInt("Died", died+1);
            foreach (GameObject eye in eyes)
            {
                eye.GetComponent<LensFlareComponentSRP>().scale = 3;
            }
            audioMixer.SetFloat("Ambient", -80);
            if (jumpscare.isPlaying == false)
            {
                jumpscare.Play();
            }
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            cameraObj.GetComponent<CustomizableCamera>().died = true;
            playerObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            playerObj.gameObject.transform.position = myPos.position;
            playerObj.gameObject.transform.rotation = myPos.rotation;
            playerObj.gameObject.transform.LookAt(focus.transform.position);
            GetComponent<CameraShake>().shakeDuration = 10f;
            GetComponent<CameraShake>().shakeMagnitude = 0.1f;

            GetComponent<CameraShake>().dampingSpeed = 0.02f;

            transform.rotation = enemyPos.rotation;
            NavMeshAgent nav = GetComponent<NavMeshAgent>();
            nav.acceleration = 100000;
            nav.speed = 0;
            nav.enabled = false;
            cameraAnim.SetTrigger("Died");
            anim.SetTrigger("Died");
            foreach (SliderSpin spin in spins)
            {
                spin.SaveVolumeButton();
            }
            yield return new WaitForSeconds(2);
            if(playerObj.GetComponent<KeepTrackOfLives>().sceneLoadCount ==3)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(3);
            }
        }
        

    }
    public IEnumerator DeathSceneManual()
    {
        foreach (GameObject eye in eyes)
        {
            eye.GetComponent<LensFlareComponentSRP>().scale = 3;
        }
        audioMixer.SetFloat("Ambient", -80);
        jumpscare.Play();
        cameraObj.GetComponent<CustomizableCamera>().died = true;
        playerObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        playerObj.GetComponent<Rigidbody>().isKinematic = true;
        playerObj.gameObject.transform.position = myPos.position;
        playerObj.gameObject.transform.rotation = myPos.rotation;        
        GetComponent<CameraShake>().shakeDuration = 10f;
        GetComponent<CameraShake>().shakeMagnitude = 0.1f;
        GetComponent<CameraShake>().dampingSpeed = 0.02f;
        GameObject prefab = Instantiate(shadowPrefab, enemyPos);
        prefab.transform.parent = null;
        Vector3 scale = new Vector3(1.4f, 1.4f, 1.4f);
        prefab.transform.localScale = scale;
        Quaternion q = new Quaternion(enemyPos.rotation.x, prefab.transform.rotation.y, enemyPos.rotation.z, enemyPos.rotation.w);
        prefab.transform.rotation = q;
        playerObj.gameObject.transform.LookAt(prefab.transform.GetChild(0).transform.position);
        //shadowPrefab.transform.rotation = enemyPos.rotation;        
        cameraAnim.SetTrigger("Died");
        anim.SetTrigger("Died");
        foreach (SliderSpin spin in spins)
        {
            spin.SaveVolumeButton();
        }        
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(3);


    }
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wood" && state == EnemyState.Roaming)
        {
            target = RandomNavSphere(origin, roamRadius, -1);
            agent.SetDestination(target);
        }
    }
    public float detectionRange;
    public float detectionAngle;
    public RaycastHit hit;
    public void Detect()
    {
        if (Vector3.Distance(transform.position, player.position) < 7)
        {
            state = EnemyState.Chasing;
            canAnimateEye = true;
        }

        Vector3 targetDir = playerPos - transform.position;
        angleToPlayer = Vector3.Angle(targetDir, transform.forward);

        if (angleToPlayer <= detectionAngle && Vector3.Distance(transform.position, player.position) < detectionRange)
        {

            
            if (Physics.Linecast(transform.position, player.transform.position, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    foreach (GameObject eyes in eyes)
                    {
                        eyes.GetComponent<LensFlareComponentSRP>().enabled = true;
                    }
                    Debug.Log("Looking at player");
                    state = EnemyState.Chasing;
                    canAnimateEye = true;
                    chasing = true;
                    i = 3;
                    lastPlayerPos = player.transform;
                }
                else
                {
                    Debug.Log("Not looking at player");
                    state = EnemyState.Roaming;
                }
            }
        }
        else
        {
            state = EnemyState.Roaming;
            foreach (GameObject eyes in eyes)
            {
                eyes.GetComponent<LensFlareComponentSRP>().enabled = false;
            }
        }
        Debug.DrawLine(eye.transform.position, player.transform.position, Color.red);
    }


    private float distanceTraveled;
    private Vector3 lastPosition;
    public float footstepDistance;
    public AudioSource audioSource;
    public void CheckForFootstep()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        distanceTraveled += distance;
        if (distanceTraveled >= footstepDistance)
        {
            float pitch = Random.Range(0.9f, 1.1f);
            PlayOnce(GetComponent<AudioSource>(), pitch);
            distanceTraveled = 0f;
        }

        lastPosition = transform.position;
    }
    public void PlayOnce(AudioSource source, float pitch)
    {
        source.pitch = pitch;
        source.Play();

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

        
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireSphere(origin, roamRadius);
    }
}
