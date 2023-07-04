using UnityEngine;

public class DoorController : MonoBehaviour
{
    public HingeJoint hingeJoint;
    public float dragSpeed = 2f;
    public float rotationThreshold = 0.1f;
    public Vector3 dragStartPosition;
    public Quaternion initialRotation;
    public bool isDragging = false;

    private void Start()
    {
        hingeJoint = GetComponent<HingeJoint>();
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 dragCurrentPosition = Input.mousePosition;
            float dragDistance = dragCurrentPosition.x - dragStartPosition.x;

            // Calculate the rotation angle based on the drag distance
            float rotationAngle = dragDistance * dragSpeed;

            // Apply the rotation to the door around the hinge axis
            Quaternion newRotation = initialRotation * Quaternion.Euler(0f, rotationAngle, 0f);
            hingeJoint.transform.rotation = newRotation;

            // Update the door's angle limits to prevent it from rotating beyond the desired range
            JointLimits limits = hingeJoint.limits;
            limits.min = hingeJoint.angle;
            limits.max = hingeJoint.angle;
            hingeJoint.limits = limits;

            // Update the drag start position for the next frame
            dragStartPosition = dragCurrentPosition;
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            isDragging = true;
            dragStartPosition = Input.mousePosition;

            // Disable physics on the door while dragging
            hingeJoint.useMotor = false;
            hingeJoint.useLimits = false;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        // Re-enable physics on the door
        hingeJoint.useMotor = true;
        hingeJoint.useLimits = true;
    }
}