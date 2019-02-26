using System.Collections;
using UnityEngine;
// using UnityEngine.VR;
using UnityEngine.XR;

public class VRToggle : MonoBehaviour
{
Quaternion offset;

void Awake()
{
    Input.gyro.enabled = true;
}

void Start()
{
    //Subtract Quaternion
    offset = transform.rotation * Quaternion.Inverse(GyroToUnity(Input.gyro.attitude));
}

void Update()
{
    GyroModifyCamera();
}

void GyroModifyCamera()
{
    //Apply offset
    transform.rotation = offset * GyroToUnity(Input.gyro.attitude);
}

private static Quaternion GyroToUnity(Quaternion q)
{
    return new Quaternion(q.x, q.y, -q.z, -q.w);
}

}