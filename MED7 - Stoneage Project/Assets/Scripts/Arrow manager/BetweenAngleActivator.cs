using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenAngleActivator : MonoBehaviour, IActivateArrow
{

    Transform camera;
    [SerializeField] private Transform target;

    Vector3 cameraForward;
    Vector3 cam2Target;

    [SerializeField] private float angleLimit, oppositeLimit;
    
    private float currentAngle;

    

    private void Awake()
    {
        camera = Camera.main.transform;

        if (!target)
            target = GameObject.Find("Arrow Pointing Position").transform;
    }



    public bool ShouldActivate()
    {
        if (camera && target)
        {

            SetCurrentAngle();

            if (currentAngle > angleLimit)
            {
                return true;
            }
        }
        
        return false;
    }

    public bool IsInOppositeDirection()
    {
        SetCurrentAngle();
        if (currentAngle > oppositeLimit)
        {
            return true;
        }
        return false;
    }

    public Transform GetPointingTarget()
    {
        return target;
    }

    private void SetCurrentAngle()
    {
        SetVectors();
        currentAngle = GetAngleBetween(cam2Target, cameraForward);
    }

    private void SetVectors()
    {
        cameraForward = camera.forward;
        cam2Target = target.position - camera.position;
    }

    private float GetAngleBetween(Vector3 from, Vector3 to)
    {
        return Vector3.Angle(from, to);
    }
}
