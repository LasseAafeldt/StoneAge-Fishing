// This solution works fine - But might not be the most optimal as the co-routine is started and stopped very frequently
namespace Assets.Scripts.IdleManager
{
    using System.Collections;
    using UnityEngine;

    public class PhoneIdle : MonoBehaviour
    {
        [Tooltip("Currently not working - Not sure Input.Acceleration does as it says")] public bool usePhoneAccelerator = false;
        [Tooltip("Detects when camera is rotating")] public bool useCameraRotation = true;
        [Tooltip("Based on boat velocity - Only works in main scene")] public bool useBoatMovement = false;
        public float timeOut = 30;

        private bool timerRunning, deviceIdle;
        private WaitForSeconds timerTime;
        private FadeController fadeController;
        private Rigidbody boatRigidBody;
        private Quaternion prevRot;
        private Camera mainCam;

        #if UNITY_ANDROID
        void Start()
        {
            timerTime = new WaitForSeconds(timeOut);
            fadeController = FindObjectOfType<FadeController>();
            mainCam = Camera.main;

            if (useBoatMovement)
                boatRigidBody = GameObject.FindGameObjectWithTag("boat")?.GetComponent<Rigidbody>();
        }

        void OnEnable()
        {
            mainCam = Camera.main;
        }

        void FixedUpdate()
        {
            if (mainCam == null)
                return;

            if (useCameraRotation)
            {
                Controller(mainCam.transform.rotation != prevRot ? 1 : 0);
                prevRot = mainCam.transform.rotation;
            }
            else if (useBoatMovement)
            {
                if (boatRigidBody != null)
                    Controller(boatRigidBody.velocity.magnitude);
            }
            else if (usePhoneAccelerator)
                Controller(Input.acceleration.magnitude);
        }

        private void Controller(float accelerationAmount)
        {
            if (accelerationAmount < 0.1f)
            {
                if (!deviceIdle && !timerRunning)
                    StartCoroutine(Timer());
            }
            else if (timerRunning)
            {
                StopAllCoroutines();
                timerRunning = false;
                FadeIn();
            }
            else
                FadeIn();
        }

        private void FadeIn()
        {
            if (!deviceIdle)
                return;

            deviceIdle = false;
            fadeController?.fadeIn();
        }

        IEnumerator Timer()
        {
            timerRunning = true;
            yield return timerTime;

            deviceIdle = true;
            fadeController?.fadeOut();
        }
        #endif
    }
}