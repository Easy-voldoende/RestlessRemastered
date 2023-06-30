using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShake : MonoBehaviour
{
    public GameObject objectToRotate;
    public RectTransform rect;
    public KeepTrackOfLives trackOfLives;
    public float shakeRange;
    public Vector3 basePos;
    public Vector3 randomPos;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        basePos = rect.position;
        randomPos = basePos;
    }
    void Update()
    {
        if(trackOfLives.sceneLoadCount == 3 && Vector3.Distance(rect.position, randomPos) < 0.1f)
        {
             randomPos = new Vector3(basePos.x +Random.Range(shakeRange,-shakeRange), basePos.y +Random.Range(shakeRange,-shakeRange), basePos.z);
        }

        rect.position = Vector3.Lerp(rect.position, randomPos, 10f);
    }
}
