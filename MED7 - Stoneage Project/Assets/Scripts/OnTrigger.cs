using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("FishAreas"))
        {
            Debug.Log(other.name + " is beign sailed towards by " + gameObject.name);
            GuidanceSounds.isSailingTowardsFishArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SoundGuide"))
        {
            Debug.Log(other.name + " is no longer being sailed towards");
            GuidanceSounds.isSailingTowardsFishArea = false;
        }
    }
}
