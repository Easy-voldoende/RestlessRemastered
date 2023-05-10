using UnityEngine;

public class CustomizableCamera : MonoBehaviour
{
    [SerializeField] private Transform player; // The player object
    [SerializeField] private float sensitivity = 2f; // The sensitivity of the mouse movement
    [SerializeField] private float minYAngle = -90f; // The minimum vertical angle the camera can rotate
    [SerializeField] private float maxYAngle = 90f; // The maximum vertical angle the camera can rotate

    private float currentXAngle = 0f; // The current horizontal angle of the camera
    private float currentYAngle = 0f; // The current vertical angle of the camera

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    private void LateUpdate()
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
}