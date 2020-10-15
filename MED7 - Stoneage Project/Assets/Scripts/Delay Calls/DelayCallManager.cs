using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayCallManager : MonoBehaviour
{
    [SerializeField] float delaySeconds;
    public bool triggerOnStart;
    public bool onlyCallOnce;

    public bool repeatCall;
    [SerializeField] float repeatSeconds;

    private bool callable = true;

    private IDelay delayResponse;

    private void Awake()
    {
        delayResponse = GetComponent<IDelay>();
    }

    private void Start()
    {
        if (triggerOnStart)
        {
            if (repeatCall)
                InvokeRepeating("RepeatCall", delaySeconds, repeatSeconds);
            else
                CallWithDelay();
        }
    }

    public void CallWithDelay()
    {
        if (delayResponse != null && callable)
        {
            StartCoroutine(CoroutineDelay(delaySeconds));
            if (onlyCallOnce) { }
                callable = false;
        }
        
    }

    private void RepeatCall()
    {
        delayResponse.Fire();
    }
    public void CancelRepeat()
    {
        CancelInvoke();
    }

    private IEnumerator CoroutineDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        delayResponse.Fire();
        
    }

}
