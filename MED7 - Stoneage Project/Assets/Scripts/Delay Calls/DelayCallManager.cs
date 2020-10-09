using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayCallManager : MonoBehaviour
{
    [SerializeField] float delaySeconds;
    public bool triggerOnStart;
    public bool onlyCallOnce;

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

    private IEnumerator CoroutineDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        delayResponse.Fire();
        
    }

}
