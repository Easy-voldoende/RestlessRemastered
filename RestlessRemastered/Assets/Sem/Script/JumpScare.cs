using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpScare : MonoBehaviour
{
    public GameObject shadowPrefab;
    public bool crawlJumpscare;
    public bool gateScare;
    GameObject shadow;
    public bool jumpscareActivated;
    public GameObject player;
    public AudioSource jumpscareAudioSource;
    public AudioSource footStepSound;
    public float angleToScare;
    public float speed = 10f;
    public Vector3 lastPosition;
    public float distanceTraveled;
    public AudioSource aud2;
    public float footstepDistance;
    public AudioSource[] otherSources;
    bool canPlay = false;
    float rotate;
    bool soundPlayed;
    void Start()
    {
        shadowPrefab.SetActive(false);
        shadow = transform.GetChild(0).gameObject;
    }


    public void LowerSound()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (!crawlJumpscare && !gateScare)
        {
            Vector3 direction;
            direction = transform.position - player.transform.position;
            angleToScare = Vector3.Angle(direction, player.transform.forward);
            if (Vector3.Distance(transform.position, player.transform.position) < 5)
            {
                if (angleToScare <= 60 && !jumpscareActivated)
                {
                    PlayOnce(jumpscareAudioSource, Random.Range(1f, 1f));
                    StartCoroutine(nameof(Scare));
                    jumpscareActivated = true;
                }
            }
        }
        else if(crawlJumpscare &&!gateScare)
        {
            Vector3 direction;
            direction = transform.position - player.transform.position;
            angleToScare = Vector3.Angle(direction, player.transform.forward);
            if (Vector3.Distance(transform.position, player.transform.position) < 15)
            {
                if (angleToScare <= 90 && !jumpscareActivated)
                {
                    StartCoroutine(nameof(Scare));
                    canPlay = true;
                    jumpscareActivated = true;
                }
            }
        }
        else if (!crawlJumpscare && gateScare)
        {
            
            if (Vector3.Distance(transform.position, player.transform.position) < 15)
            {
                if (angleToScare <= 90 && !jumpscareActivated)
                {
                    PlayOnce(jumpscareAudioSource, Random.Range(0.9f, 1.1f));
                    StartCoroutine(nameof(GateScare));
                    StartCoroutine(nameof(Adjust));
                    StartCoroutine(nameof(Rotate));
                    canPlay = true;
                    jumpscareActivated = true;
                }
            }
        }
        if (jumpscareActivated && crawlJumpscare)
        {
            CrawlScare();
        }

        if (jumpscareActivated && gateScare)
        {
            Rotate();
        }
    }
    public void PlayOnce(AudioSource source, float pitch)
    {
        source.pitch = pitch;
        source.Play();

    }
    public IEnumerator Scare()
    {
        shadowPrefab.SetActive(true);
        yield return new WaitForSeconds(1f);
        shadowPrefab.SetActive(false);
        gameObject.GetComponent<JumpScare>().enabled = false;
        canPlay = false;
    }
    public void CrawlScare()
    {
        shadowPrefab.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if(soundPlayed == false)
        {
            PlayOnce(aud2, 1);
            soundPlayed = true;
        }
        CheckForFootstep();

    }
    public void Rotate()
    {
        gameObject.transform.Rotate(0, 0, rotate * Time.deltaTime);
    }
    public IEnumerator GateScare()
    {
        StartCoroutine(nameof(Adjust));
        shadowPrefab.SetActive(true);
        yield return new WaitForSeconds(1f);
        shadowPrefab.SetActive(false);
        gameObject.GetComponent<JumpScare>().enabled = false;
        canPlay = false;
    }
    public IEnumerator Adjust()
    {
        rotate = 140;
        yield return new WaitForSeconds(0.15f);
        rotate = 0;
        yield return new WaitForSeconds(0.4f);
        rotate = -100;
    }
    public void CheckForFootstep()
    {
        if (canPlay)
        {
            float distance = Vector3.Distance(shadow.transform.position, lastPosition);
            distanceTraveled += distance;
            if (distanceTraveled >= footstepDistance)
            {
                float pitch = Random.Range(0.7f, 0.9f);
                PlayOnce(footStepSound, pitch);

                distanceTraveled = 0f;

            }

            lastPosition = shadow.transform.position;
        }

    }
}
