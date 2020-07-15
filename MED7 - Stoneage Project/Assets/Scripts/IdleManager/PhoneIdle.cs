namespace Assets.Scripts.IdleManager
{
    using System.Collections;
    using UnityEngine;

    public class PhoneIdle : MonoBehaviour
    {
        [Tooltip("IF FALSE: for now uses rigidbody velocity of boat - But should be based on something else")] public bool usePhoneAccelerator = true;
        public float timeOut = 30;

        private bool timerRunning, deviceIdle;
        private WaitForSeconds timerTime;
        private FadeController fadeController;
        private Rigidbody boatRigidBody;

        #if UNITY_ANDROID
        void Start()
        {
            timerTime = new WaitForSeconds(timeOut);
            fadeController = FindObjectOfType<FadeController>();

            if (!usePhoneAccelerator)
                boatRigidBody = GameObject.FindGameObjectWithTag("boat")?.GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (!usePhoneAccelerator)
            {
                if (boatRigidBody != null)
                    Controller(boatRigidBody.velocity.magnitude);
            }
            else
                Controller(Input.acceleration.magnitude);
        }

        private void Controller(float accelerationAmount)
        {
            if (accelerationAmount < 0.2f)
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