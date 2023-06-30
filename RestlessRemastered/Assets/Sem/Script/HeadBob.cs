using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float bobbingSpeed = 0.3f;
    public float bobbingAmount = 0.25f;
    public GameObject player;
    public float horizontal;
    public float vertical;
    public float speed;
    public bool isCutscene;
    private float timer = 0.0f;
    private float midpoint = 0.0f;

    private Vector3 originalPosition;
    public float baseBobbingSpeed;

    private void Start()
    {
        originalPosition = transform.localPosition;
        if (isCutscene == true)
        {
            midpoint = 0.11f;
        }
        else
        {
            midpoint = 0.8f;
        }

        baseBobbingSpeed = 55;
    }

    private void Update()
    {
        if (player != null)
        {
            bobbingSpeed = baseBobbingSpeed * player.GetComponent<Rigidbody>().velocity.magnitude * speed;
        }
        else
        {
            bobbingSpeed = 0.035f;
        }

        float waveslice = 0.0f;

        if (!isCutscene)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
        else
        {
            horizontal = 1;
            vertical = 1;
        }

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            float wavesliceSpeed = bobbingSpeed * Time.deltaTime;
            timer += wavesliceSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer -= Mathf.PI * 2;
            }
        }

        if (timer != 0)
        {
            waveslice = Mathf.Sin(timer);
            float translateChange = waveslice * bobbingAmount;

            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange *= totalAxes;

            Vector3 localPosition = originalPosition;
            localPosition.y = midpoint + translateChange;
            transform.localPosition = localPosition;
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }
}