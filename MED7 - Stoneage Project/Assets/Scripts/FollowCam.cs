using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public Transform objToFollow;
    public float smoothTime = 0.3f;
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    protected virtual void LateUpdate()
    {
        SmoothFollow();
    }

    protected void SmoothFollow()
    {
        Vector3 targetPos = objToFollow.position;
        Vector3 currentPos = Vector3.SmoothDamp(transform.position, objToFollow.position, ref velocity, smoothTime * Time.deltaTime);
        transform.position = currentPos;
    }
}
