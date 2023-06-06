using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(Nextscene));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator Nextscene()
    {
        yield return new WaitForSeconds(23.3f);
        SceneManager.LoadScene(1);
    }
}
