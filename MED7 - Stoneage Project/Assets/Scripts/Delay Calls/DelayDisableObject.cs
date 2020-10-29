using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDisableObject : MonoBehaviour, IDelay
{
    [SerializeField] private GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        obj.SetActive(true);
    }

    public void Fire()
    {
        obj.SetActive(false);
    }

}
