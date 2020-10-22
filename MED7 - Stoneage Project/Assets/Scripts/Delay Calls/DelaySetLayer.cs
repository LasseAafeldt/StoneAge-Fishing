using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelaySetLayer : MonoBehaviour, IDelay
{
    [SerializeField] private string layerName;
    [SerializeField] private GameObject objectToSetLayerOn;

    public void Fire()
    {
        objectToSetLayerOn.layer = ChangeToNewLayer(layerName);
    }

    private int ChangeToNewLayer(string layerName)
    {
        return LayerMask.NameToLayer(layerName);
    }

}
