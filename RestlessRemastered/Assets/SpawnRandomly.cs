using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomly : MonoBehaviour
{
    public GameObject ScrewDriver;
    public GameObject Pin;
    public GameObject[] spawnObjects;
    public bool spawnedScrewdriver;
    public bool spawnedPin;
    public int objectsSpawned;
    int usedSpot;
    int spawnSpot;
    void Start()
    {
        if(objectsSpawned == 0)
        {
            spawnSpot = 9;//Random.Range(0, spawnObjects.Length);
            Instantiate(ScrewDriver, spawnObjects[spawnSpot].transform);
            objectsSpawned = 1;
            usedSpot = spawnSpot;
            spawnSpot = Random.Range(0, spawnObjects.Length);
            if(spawnSpot == usedSpot)
            {
                spawnSpot -= 2;
            }
            
            
            Instantiate(Pin, spawnObjects[spawnSpot].transform);
            objectsSpawned = 2;
        }
    }
}
