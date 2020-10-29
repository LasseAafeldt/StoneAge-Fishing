using UnityEngine;

public class BoatControllerScript : MonoBehaviour
{

    public static string currentlyInRegion = "No Region";

    public float speed = 15f, rotationSpeed = 0.1f, rotationDeadzone = 5f, accelerationAmount = 0.05f, accelerationDeadzone = 90f, rowingFrequency = 0.44f;
    public Collider[] ignoreCollision;

    private bool outOfBounds = false;
    private bool rowFXPlaying = false;
    private bool shouldMove = true;
    private float acceleration;

    private AudioSource paddleSource;
    private Camera mainCamera;
    private PartnerAnimator animator;
    private Rigidbody boatRB;

    private void Start()
    {
        outOfBounds = false;
        rowFXPlaying = false;
        shouldMove = true;
        CacheComponents();
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
        if (Input.GetButtonDown("Fire1") && !outOfBounds && !GameManager.singleton.pointingAtInteractable)
        {
            EnableSailing();
            Debug.Log("I got here");
        }
        else if (Input.GetButtonUp("Fire1") && !outOfBounds)
        {
            DisableSailing();
        }

        if (Input.GetButton("Fire1") && !outOfBounds)
        {
            float sinusoid = (Mathf.Sin(2.0f * Mathf.PI * rowingFrequency * Time.time+1) + 1.0f / 2.0f); // Asin(2 * PI * Frequency * time) + phase
            if (sinusoid < 0.25f)
                sinusoid = 0.25f;

            if (acceleration < 1.0f)
                acceleration += accelerationAmount;

            if (!GameManager.singleton.pointingAtInteractable)
            {
                float rotationAngle = Vector3.Angle(transform.forward, mainCamera.transform.forward);

                if (animator.anim.GetBool("isRowing") && rotationAngle < accelerationDeadzone)
                {
                    boatRB
                        .AddForce((transform.forward * speed * sinusoid * Time.deltaTime) * acceleration);
                    Debug.Log("Should row forwards    " + Time.time);
                }

                if (rotationAngle > rotationDeadzone)
                {
                    Vector3 newDir = Vector3.RotateTowards(transform.forward,
                        new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z), rotationAngle,
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

    private void EnableSailing()
    {
        rowFXPlaying = true;
        paddleSource.Play();
        animator.paddleAnimation(true);
        shouldMove = true;
    }

    private void DisableSailing()
    {
        rowFXPlaying = false;
        paddleSource.Stop();
        animator.paddleAnimation(false);
        shouldMove = false;
    }

    private void CacheComponents()
    {
        paddleSource = GameManager.singleton.paddle.GetComponent<AudioSource>();
        animator = GameManager.singleton.partner.GetComponent<PartnerAnimator>();
        mainCamera = Camera.main;
        boatRB = GetComponent<Rigidbody>();
    }
}