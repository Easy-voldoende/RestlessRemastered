using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    void Update()
    {
        PlayHeartBeat();
    }
    public float heartBeatCooldown;
    public float maxCooldown = 10;
    public AudioSource heartBeat;
    public GameObject shadow;
    public void PlayHeartBeat()
    {
        float dist = Vector3.Distance(transform.position, shadow.transform.position);
        float maxDist = 20;
        float value = maxDist - dist;
        if (dist <= maxDist)
        {

            heartBeatCooldown += 0.5f * value * Time.deltaTime * 2;

            if (heartBeatCooldown >= maxCooldown)
            {
                PlayOnce(heartBeat, 1);
                heartBeatCooldown = 0;
            }
        }
    }

    public void PlayOnce(AudioSource source, float pitch)
    {
        source.pitch = pitch;
        source.Play();

    }
}
