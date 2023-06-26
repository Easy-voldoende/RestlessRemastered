using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class PlayerMovementGrappling : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public bool lightOn;
    public GameObject lightBulb;
    public AudioSource secondFootStep;
    public AudioSource secondFootStepTwo;
    public LayerMask walkingSufaces;
    public bool inFootstepArea;
    public float walkSpeed;
    public float sprintSpeed;
    public AudioSource[] gravelFootstepClips;
    public AudioSource[] gravelFootstepClipsSprinting;
    public AudioSource[] woodFootstepClips;
    public AudioSource[] woodFootstepsSprinting;
    public AudioSource[] grassFootstepClips;
    public AudioSource[] grassFootstepsSprinting;
    public AudioSource[] rockFootstepsSprinting;
    public AudioSource[] rockFootstepsWalking;
    public AudioSource[] rockJump;
    public Vector3 enemySpawnPos;
    public GameObject enemySpawn;
    public AudioSource[] grassJump;
    public AudioSource[] woodJump;
    public AudioSource[] gravelJump;
    public float footstepDistance;
    public float heartBeatSpeed =1;

    
    public float distanceTraveled;
    private Vector3 lastPosition;

    private AudioSource audioSource;
    public AudioSource heartBeat;

    public float groundDrag;
    
    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Camera Effects")]
    public Camera cam;
    public float grappleFov = 95f;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    public HeadBob headBob;
    Vector3 moveDirection;
    public string curWalkingSurface;
    Rigidbody rb;
    float mass;

    public MovementState state;
    public enum MovementState
    {
        freeze,
        grappling,
        swinging,
        walking,
        sprinting,
        crouching,
        air
    }

    public bool freeze;

    private void Start()
    {
        mass = GetComponent<Rigidbody>().mass;
        headBob = GameObject.Find("Holder").GetComponent<HeadBob>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }
    RaycastHit hitcast;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ManualJumpScare();

        }
        // ground check
        if (Input.GetKeyDown(KeyCode.F))
        {
            FlashLight();
        }
        grounded = Physics.Raycast(transform.position, Vector3.down,out hitcast, playerHeight * 0.5f + 0.3f, whatIsGround);
        
        if(Physics.Raycast(transform.position, Vector3.down, out hitcast, playerHeight * 0.5f + 0.3f, whatIsGround))
        {
            curWalkingSurface = hitcast.transform.gameObject.tag;
        }

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        if (grounded)
        {
            CheckForFootstep();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ChangeSpeed();
        }
    }

    public void ManualJumpScare()
    {
        GameObject shadow = GameObject.Find("RealShadow");
        shadow.GetComponent<EnemyPathfinding>().manual = true;
        shadow.GetComponent<NavMeshAgent>().enabled = false;
        enemySpawnPos = enemySpawn.transform.position;
        shadow.transform.position = enemySpawnPos;
        StartCoroutine(shadow.GetComponent<EnemyPathfinding>().DeathScene());
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        // Mode - Freeze
        if (freeze)
        {
            state = MovementState.freeze;
            moveSpeed = 0;
            rb.velocity = Vector3.zero;
            GetComponent<Rigidbody>().mass = mass*3;
        }

        // Mode - Crouching
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            
            GetComponent<Rigidbody>().mass = mass;
            moveSpeed = crouchSpeed;
        }
        if (Input.GetKeyDown(crouchKey))
        {
            rb.AddForce(Vector3.down* 5f, ForceMode.Impulse);
        }
        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            GetComponent<Rigidbody>().mass = mass;
            headBob.bobbingAmount = 0.05f;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            GetComponent<Rigidbody>().mass = mass;
            headBob.bobbingAmount = 0.05f;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
            headBob.bobbingAmount = 0f;
        }
    }

    private void MovePlayer()
    {

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        if(curWalkingSurface == "Wood")
        {
            float pitch = Random.Range(0.9f, 1.1f);
            int audioSourceInt = Random.Range(0, woodJump.Length);
            PlayOnce(woodJump[audioSourceInt], pitch);
        }

        if(curWalkingSurface == "Grass")
        {
            float pitch = Random.Range(0.9f, 1.1f);
            int audioSourceInt = Random.Range(0, grassJump.Length);
            PlayOnce(grassJump[audioSourceInt], pitch);
        }

        if(curWalkingSurface == "Gravel")
        {
            float pitch = Random.Range(0.9f, 1.1f);
            int audioSourceInt = Random.Range(0, gravelJump.Length);
            PlayOnce(gravelJump[audioSourceInt], pitch);
        }
        if (curWalkingSurface == "Rock")
        {
            float pitch = Random.Range(0.9f, 1.1f);
            int audioSourceInt = Random.Range(0, rockJump.Length);
            PlayOnce(rockJump[audioSourceInt], pitch);
        }

    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }  

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
    public void CheckForFootstep()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        distanceTraveled += distance;
        if(state == MovementState.walking)
        {
            if (distanceTraveled >= footstepDistance)
            {
                if (curWalkingSurface == "Gravel")
                {
                    float pitch = Random.Range(0.9f, 1.1f);
                    int audioSourceInt = Random.Range(0, gravelFootstepClips.Length);
                    PlayOnce(gravelFootstepClips[audioSourceInt], pitch);
                    distanceTraveled = 0f;
                    Debug.Log(curWalkingSurface);
                }

                if (curWalkingSurface == "Wood")
                {
                    float pitch = Random.Range(0.9f, 1.1f);
                    int audioSourceInt = Random.Range(0, woodFootstepClips.Length);
                    PlayOnce(woodFootstepClips[audioSourceInt], pitch);
                    distanceTraveled = 0f;
                    Debug.Log(curWalkingSurface);
                }

                if (curWalkingSurface == "Grass")
                {
                    float pitch = Random.Range(0.9f, 1.1f);
                    int audioSourceInt = Random.Range(0, grassFootstepClips.Length);
                    PlayOnce(grassFootstepClips[audioSourceInt], pitch);
                    distanceTraveled = 0f;
                    Debug.Log(curWalkingSurface);
                }

                if (curWalkingSurface == "Rock")
                {
                    float pitch = Random.Range(0.9f, 1.1f);
                    int audioSourceInt = Random.Range(0, rockFootstepsWalking.Length);
                    PlayOnce(rockFootstepsWalking[audioSourceInt], pitch);
                    distanceTraveled = 0f;
                    Debug.Log(curWalkingSurface);
                }
                if (inFootstepArea)
                {
                    int i = Random.Range(1, 10);
                    int j = Random.Range(0, 2);
                    if (i == 1)
                    {
                        if (j == 0)
                        {
                            StartCoroutine(nameof(SecondStep));
                            Debug.Log("StepOne");
                        }

                        if (j == 1)
                        {
                            StartCoroutine(nameof(SecondStepTwo));
                            Debug.Log("StepTwo");
                        }
                    }
                }
            }
        }
        else if(state == MovementState.sprinting)
        {
            if (distanceTraveled >= footstepDistance)
            {
                if (curWalkingSurface == "Gravel")
                {
                    float pitch = Random.Range(0.9f, 1.1f);
                    int audioSourceInt = Random.Range(0, gravelFootstepClipsSprinting.Length);
                    PlayOnce(gravelFootstepClipsSprinting[audioSourceInt], pitch);
                    distanceTraveled = 0f;
                    Debug.Log(curWalkingSurface);
                }

                if (curWalkingSurface == "Wood")
                {
                    float pitch = Random.Range(0.9f, 1.1f);
                    int audioSourceInt = Random.Range(0, woodFootstepsSprinting.Length);
                    PlayOnce(woodFootstepsSprinting[audioSourceInt], pitch);
                    distanceTraveled = 0f;
                    Debug.Log(curWalkingSurface);
                }

                if (curWalkingSurface == "Grass")
                {
                    float pitch = Random.Range(0.9f, 1.1f);
                    int audioSourceInt = Random.Range(0, grassFootstepsSprinting.Length);
                    PlayOnce(grassFootstepsSprinting[audioSourceInt], pitch);
                    distanceTraveled = 0f;
                    Debug.Log(curWalkingSurface);
                }
                if (curWalkingSurface == "Rock")
                {
                    float pitch = Random.Range(0.9f, 1.1f);
                    int audioSourceInt = Random.Range(0, rockFootstepsSprinting.Length);
                    PlayOnce(rockFootstepsSprinting[audioSourceInt], pitch);
                    distanceTraveled = 0f;
                    Debug.Log(curWalkingSurface);
                }
                if (inFootstepArea)
                {
                    int i = Random.Range(1, 10);
                    int j = Random.Range(0, 2);
                    if (i == 1)
                    {
                        if (j == 0)
                        {
                            StartCoroutine(nameof(SecondStep));
                            Debug.Log("StepOne");
                        }

                        if (j == 1)
                        {
                            StartCoroutine(nameof(SecondStepTwo));
                            Debug.Log("StepTwo");
                        }
                    }
                }
            }
        }

        lastPosition = transform.position;
        
    }
    public IEnumerator FlashlightUI()
    {
        yield return new WaitForSeconds(6);

    }
    public void FlashLight()
    {
        if(lightOn == false)
        {
            lightBulb.GetComponent<Light>().enabled = true;
            lightOn = true;
            PlayOnce(lightBulb.GetComponent<AudioSource>(), Random.Range(1.1f, 1.2f));
        }
        else
        {
            lightBulb.GetComponent<Light>().enabled = false;
            lightOn = false;
            PlayOnce(lightBulb.GetComponent<AudioSource>(), Random.Range(0.8f, 0.9f));
        }

 
    }
    public void ChangeSpeed()
    {
        if(sprintSpeed == 9.45f)
        {
            sprintSpeed = 20;
        }else if(sprintSpeed == 20)
        {
            sprintSpeed = 9.45f;
        }
    }
    public IEnumerator SecondStep()
    {
        yield return new WaitForSeconds(0.2f);
        PlayOnce(secondFootStep, 1);

    }
    public IEnumerator SecondStepTwo()
    {
        yield return new WaitForSeconds(0.2f);
        PlayOnce(secondFootStepTwo, 1);

    }
    public void PlayOnce(AudioSource source, float pitch)
    {
        source.pitch = pitch;
        source.Play();

    }
    
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) 
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }
    

}
