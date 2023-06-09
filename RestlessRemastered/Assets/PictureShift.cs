using UnityEngine;

public class PictureShift : MonoBehaviour
{
    // Define an array to store the initial rotations of each gameObject
    private Quaternion[] initialRotations;
    private bool hasRotated;
    public bool solved;
    public LayerMask mask;

    private void Start()
    {
        // Store the initial rotations of each gameObject
        initialRotations = new Quaternion[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject puzzlePiece = transform.GetChild(i).gameObject;

            // Assign a random rotation to the puzzle piece
            Quaternion randomRotation = Quaternion.Euler(Random.Range(0, 4) * 90f, 0f, 0f);
            puzzlePiece.transform.rotation = randomRotation;

            initialRotations[i] = puzzlePiece.transform.rotation;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit,2f,mask))
            {
                // Check if the raycast hit one of the puzzle pieces
                GameObject puzzlePiece = hit.collider.gameObject;
                if (puzzlePiece.transform.IsChildOf(transform))
                {
                    // Rotate the puzzle piece upwards by 90 degrees
                    puzzlePiece.transform.Rotate(Vector3.right, -90f);

                    // Set the flag to indicate that the player has made a rotation
                    hasRotated = true;
                }
            }
        }

        // Check if the puzzle is solved only after the player has made a rotation
        if (hasRotated && CheckPuzzleSolved())
        {
            
            if(solved == true)
            {
                Debug.Log("Puzzle Solved!");
                solved = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject puzzlePiece = transform.GetChild(i).gameObject;

                // Assign a random rotation to the puzzle piece
                Quaternion randomRotation = Quaternion.Euler(Random.Range(0, 4) * 90f, 0f, 0f);
                puzzlePiece.transform.rotation = randomRotation;

                initialRotations[i] = puzzlePiece.transform.rotation;
            }
        }
    }

    private bool CheckPuzzleSolved()
    {
        // Check if any gameObjects have rotations other than 0,0,0,0
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject puzzlePiece = transform.GetChild(i).gameObject;
            if (puzzlePiece.transform.rotation.eulerAngles != Vector3.zero)
            {
                solved = true;
                return false;
                
            }
        }

        return true;
    }
}