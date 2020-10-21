using UnityEngine;

public class BoatControllerScript : MonoBehaviour
{

    public static string currentlyInRegion = "No Region";

    public float speed = 15f, rotationSpeed = 0.1f, rotationDeadzone = 5f, accelerationAmount = 0.05f, accelerationDeadzone = 90f, rowingFrequency = 0.44f;
    public Collider[] ignoreCollision;

    bool voiceLineReady = true;

    float verticalInput, horizontalInput, steerFactor, acceleration;

    private bool outOfBounds = false;

    private void Start()
    {
        outOfBounds = false;
        voiceLineReady = true;
    }

    void Update()
    {
        if (GameManager.singleton.canMove)
        {
            BoatMovement();
        }
    }

    public void BoatMovement()
    {
        if (Input.GetButtonDown("Fire1") && !outOfBounds && !GameManager.singleton.pointingAtInteractable && GameManager.singleton.canMove)
        {
            GameManager.singleton.paddle.GetComponent<AudioSource>().Play();
            GameManager.singleton.partner.GetComponent<PartnerAnimator>().paddleAnimation(true);
        }
        else if (Input.GetButtonUp("Fire1") && !outOfBounds)
        {
            GameManager.singleton.paddle.GetComponent<AudioSource>().Stop();
            GameManager.singleton.partner.GetComponent<PartnerAnimator>().paddleAnimation(false);
        }

        if (Input.GetButton("Fire1") && !outOfBounds)
        {
            //float sinusoid = (Mathf.Sin(Time.time) + 1.0f / 2.0f);
            float sinusoid = (Mathf.Sin(2.0f * Mathf.PI * rowingFrequency * Time.time+1) + 1.0f / 2.0f); // Asin(2 * PI * Frequency * time) + phase
            if (sinusoid < 0.25f)
            {
                sinusoid = 0.25f;
            }

            if (acceleration < 1.0f)
                acceleration += accelerationAmount;

            if (!GameManager.singleton.pointingAtInteractable)
            {
                float rotationAngle = Vector3.Angle(transform.forward, Camera.main.transform.forward);

                if (GameManager.singleton.partner.GetComponent<PartnerAnimator>().anim.GetBool("isRowing") && rotationAngle < accelerationDeadzone)
                {
                    GetComponent<Rigidbody>()
                        .AddForce((transform.forward * speed * sinusoid * Time.deltaTime) * acceleration);
                }

                if (rotationAngle > rotationDeadzone)
                {
                    Vector3 newDir = Vector3.RotateTowards(transform.forward,
                        new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z), rotationAngle,
                        0.0f);

                    // calculate the Quaternion for the rotation
                    float deltaAngle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(newDir));

                    Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newDir),
                       rotationSpeed * Time.deltaTime);

                    // Vector3 camDirectionNoY = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
                    // Quaternion rot = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(camDirectionNoY), rotationSpeed * Time.deltaTime);

                    //Apply the rotation 
                    transform.rotation = rot;

                }
            }
        }
        else
            acceleration = accelerationAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Regions"))
            return;
        setCurrentRegion(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Regions"))
            return;
        resetCurrentRegion();
    }

    void setCurrentRegion(GameObject region)
    {
        currentlyInRegion = region.name;
    }

    public string getCurrentRegion()
    {
        return currentlyInRegion;
    }

    void resetCurrentRegion()
    {
        //set curentregion to no region
        currentlyInRegion = "No Region";
        //if multiple colliders is use to make one region, then check if other region colliders overlap with current position
    }
}