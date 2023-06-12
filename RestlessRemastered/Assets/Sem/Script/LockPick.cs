using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPick : MonoBehaviour
{
    public bool startedPicking;
    public GameObject pin;
    public bool isActive;
    public bool picked;
    public GameObject camPos;
    public GameObject door;
    public GameObject door2;
    public Camera cam;
    public Transform innerLock;
    public Transform pickPosition;
    public GameObject screwDriver;
    public float maxAngle = 90;
    public float lockSpeed = 10;
    public AudioSource pickingSound;
    public AudioSource openTheNoor;
    
    [Range(1, 25)]
    public float lockRange = 10;

    private float eulerAngle;
    private float unlockAngle;
    private Vector2 unlockRange;

    private float keyPressTime = 0;
    Color color;
    private bool movePick = true;

    // Start is called before the first frame update
    void Start()
    {
        NewLock();
        color = pin.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(picked == false)
        {
            transform.position = pickPosition.position;
            if (startedPicking == true)
            {


                if (movePick)
                {
                    Vector3 dir = Input.mousePosition - cam.WorldToScreenPoint(transform.position);

                    eulerAngle = Vector3.Angle(dir, Vector3.up);

                    Vector3 cross = Vector3.Cross(Vector3.up, dir);
                    if (cross.z < 0) { eulerAngle = -eulerAngle; }

                    eulerAngle = Mathf.Clamp(eulerAngle, -maxAngle, maxAngle);

                    Quaternion rotateTo = Quaternion.AngleAxis(eulerAngle, Vector3.forward);
                    transform.localRotation = rotateTo;
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    movePick = false;
                    PlayOnce(pickingSound,1);
                    keyPressTime = 1;
                }
                if (Input.GetKeyUp(KeyCode.D))
                {
                    movePick = true;
                    StopSound(pickingSound);
                    keyPressTime = 0;
                }

                float percentage = Mathf.Round(100 - Mathf.Abs(((eulerAngle - unlockAngle) / 100) * 100));
                float lockRotation = ((percentage / 100) * maxAngle) * keyPressTime;
                float maxRotation = (percentage / 100) * maxAngle;

                float lockLerp = Mathf.Lerp(innerLock.localEulerAngles.z, lockRotation, Time.deltaTime * lockSpeed);
                innerLock.localEulerAngles = new Vector3(0, 0, lockLerp);

                if (lockLerp >= maxRotation - 1)
                {
                    if (eulerAngle < unlockRange.y && eulerAngle > unlockRange.x)
                    {
                        Debug.Log("Unlocked!");
                        NewLock();
                        EndPicking();
                        picked = true;
                        movePick = false;
                        keyPressTime = 0;
                        
                    }
                    else
                    {
                        float randomRotation = Random.insideUnitCircle.x;
                        transform.localEulerAngles += new Vector3(0, 0, Random.Range(-randomRotation, randomRotation));
                    }
                }
            }
        }

        
    }
    public void StopSound(AudioSource source)
    {
        source.Stop();
    }
    public void PlayOnce(AudioSource source, float pitch)
    {
        source.pitch = pitch;
        source.Play();

    }
    void NewLock()
    {
        unlockAngle = Random.Range(-maxAngle + lockRange, maxAngle - lockRange);
        unlockRange = new Vector2(unlockAngle - lockRange, unlockAngle + lockRange);
    }

    public void EndPicking()
    {
        GameObject player = GameObject.Find("Player");
        GameObject camera = GameObject.Find("Main Camera");
        PlayOnce(openTheNoor, 1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(nameof(ResetPlayerPos));
        
    }

    public IEnumerator ResetPlayerPos()
    {
        
        
        GameObject camera = GameObject.Find("Main Camera");
        GameObject player = GameObject.Find("Player");
        Rigidbody rb = player.GetComponent<Rigidbody>();
        yield return new WaitForSeconds(1);
        pin.SetActive(false);
        screwDriver.SetActive(false);
        player.transform.position = camera.GetComponent<CheckForLock>().lastPos.position;
        door.GetComponent<Animator>().SetTrigger("Door");
        player.GetComponent<PlayerMovementGrappling>().enabled = true;
        camera.GetComponent<CustomizableCamera>().enabled = true;
        rb.isKinematic = false;
        door2.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.SetActive(false);
        yield return new WaitForSeconds(0.75f);
        
    }

}
