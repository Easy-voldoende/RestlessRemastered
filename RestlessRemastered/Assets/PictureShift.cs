using UnityEngine;

public class PictureShift : MonoBehaviour
{
    public Vector3[] newRotations;
    public Vector3[] startRotations;
    private bool hasRotated;
    public bool solved;
    public LayerMask mask;
    public AudioSource source;

    private void Start()
    {
        newRotations = new Vector3[transform.childCount];
        startRotations = new Vector3[transform.childCount];
        NewRot();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit,2f,mask))
            {
                GameObject puzzlePiece = hit.collider.gameObject;
                if (puzzlePiece.transform.IsChildOf(transform))
                {
                    puzzlePiece.transform.Rotate(Vector3.right, -90f);
                    PlayOnce(source, Random.Range(0.7f, 0.9f));
                    hasRotated = true;
                }
            }
        }

        if (hasRotated && CheckPuzzleSolved())
        {
            
            if(solved == true)
            {
                Debug.Log("Puzzle Solved!");
                solved = false;
            }
        }

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    NewRot();
        //}
    }
    public void NewRot()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject puzzlePiece = transform.GetChild(i).gameObject;
            startRotations[i] = puzzlePiece.transform.localEulerAngles;

            Vector3 randomRotation = new Vector3(Random.Range(1, 4) * 90, 0, 0);
            puzzlePiece.transform.localEulerAngles = randomRotation;

            newRotations[i] = puzzlePiece.transform.localEulerAngles;
        }
    }
    public void PlayOnce(AudioSource source, float pitch)
    {
        source.pitch = pitch;
        source.Play();

    }
    public bool CheckPuzzleSolved()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject puzzlePiece = transform.GetChild(i).gameObject;
            if (puzzlePiece.transform.localEulerAngles != Vector3.zero)
            {
                solved = false;
                return false;                
            }
        }

        solved = true;
        return true;
    }
}