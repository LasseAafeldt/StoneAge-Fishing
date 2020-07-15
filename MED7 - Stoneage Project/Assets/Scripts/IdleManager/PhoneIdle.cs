namespace Assets.Scripts.IdleManager
{
    using System.Collections;
    using UnityEngine;

    public class PhoneIdle : MonoBehaviour
    {
        public float timeOut = 10;

        private bool timerRunning, deviceIdle;
        private WaitForSeconds timerTime;
        private FadeController fadeController;

        #if UNITY_ANDROID
        void Start()
        {
            timerTime = new WaitForSeconds(timeOut);
            fadeController = FindObjectOfType<FadeController>();
        }

        void Update()
        {
            Controller(Input.acceleration.magnitude);
        }

        private void Controller(float accelerationAmount)
        {
            if (deviceIdle) return;

            if (accelerationAmount < 8.0f)
                StartCoroutine(Timer());
            else if (timerRunning)
            {
                StopAllCoroutines();
                timerRunning = false;
                fadeController?.fadeIn();
            }
        }

        IEnumerator Timer()
        {
            timerRunning = true;
            yield return timerTime;

            fadeController?.fadeOut();
        }
        #endif
    }
}