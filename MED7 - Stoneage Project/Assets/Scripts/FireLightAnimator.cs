using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLightAnimator : MonoBehaviour {

    //public GameObject objToAnimate;
    public float smoothTime = 0.2f;
    [Range(0f,2f)]
    public float radiusOfAnimation = 1;

    Vector3 startPosition;
    Vector3 localTargetPosition;
    Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        setTargetPosition();
        moveObj();
    }

    void setTargetPosition()
    {
        localTargetPosition = new Vector3(Random.Range(0, radiusOfAnimation), Random.Range(0, radiusOfAnimation), Random.Range(0, radiusOfAnimation));
        //clamp the magnitude of the vector to the max radius
        localTargetPosition = Vector3.ClampMagnitude(localTargetPosition, radiusOfAnimation);
        //add the start position to the local position to make sure the object has a target near itself
        targetPosition = new Vector3(localTargetPosition.x + startPosition.x,
            localTargetPosition.y + startPosition.y,
            localTargetPosition.z + startPosition.z);
    }

    void moveObj()
    {
        Vector3 currentPos = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime * Time.deltaTime);
        transform.position = currentPos;
    }
}
