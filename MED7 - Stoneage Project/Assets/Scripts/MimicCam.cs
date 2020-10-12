using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicCam : FollowCam
{
    public float smoothRotateTime =0.3f;

    override protected void LateUpdate()
    {
        SmoothFollow();
        SmoothRotate();
    }


    protected void SmoothRotate()
    {
        Quaternion targetRot = objToFollow.rotation;
        Quaternion currentRot = Quaternion.Slerp(transform.rotation, objToFollow.rotation, smoothRotateTime * Time.deltaTime);
        transform.rotation = currentRot;
    }


}
