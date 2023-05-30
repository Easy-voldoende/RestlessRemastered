using UnityEngine;

public class CustomizableCamera : MonoBehaviour
{
    [SerializeField] private Transform player; // The player object
    [SerializeField] private float sensitivity = 2f; // The sensitivity of the mouse movement
    [SerializeField] private float minYAngle = -90f; // The minimum vertical angle the camera can rotate
    [SerializeField] private float maxYAngle = 90f; // The maximum vertical angle the camera can rotate
    public bool died;
    public Transform orientation;
    public bool sceneStarted;
    public GameObject movement;

    private float currentXAngle = 0f; // The current horizontal angle of the camera
    private float currentYAngle = 0f; // The current vertical angle of the camera

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    private void LateUpdate()
    {
        if(died == false && sceneStarted == false)
        {
            // Get the mouse input
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            // Rotate the camera based on the mouse input
            currentXAngle += mouseX;
            currentYAngle -= mouseY;

            // Clamp the vertical angle to the specified range
            currentYAngle = Mathf.Clamp(currentYAngle, minYAngle, maxYAngle);

            // Rotate the player object horizontally based on the mouse input
            player.Rotate(Vector3.up * mouseX);

            // Rotate the camera vertically based on the mouse input
            transform.localRotation = Quaternion.Euler(currentYAngle, 0f, 0f);
        }
        else if(sceneStarted == false&& died ==true) 
        {
            PlayerDeathAnim();
            sceneStarted = true;
        }
    }

    public void PlayerDeathAnim()
    {
        GameObject shadow = GameObject.Find("FocusPoint").gameObject;
        movement.GetComponent<PlayerMovementGrappling>().enabled = false;
        
        gameObject.transform.LookAt(shadow.transform.position);
        Quaternion q = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        gameObject.transform.rotation = q;
    }

}