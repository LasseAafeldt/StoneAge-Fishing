using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayEnableObject : MonoBehaviour, IDelay
{
    [SerializeField] private GameObject disabledObject;


    private void Start()
    {
        disabledObject.SetActive(false);
    }

    public void Fire()
    {
        disabledObject.SetActive(true);
    }

}
