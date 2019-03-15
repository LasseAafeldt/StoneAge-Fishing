using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCircler : MonoBehaviour {

    public Transform objToCircle;
    public float speed = 15;
    public bool forwardDirection;

    private void FixedUpdate()
    {
        if (forwardDirection)
        {

            transform.RotateAround(objToCircle.position, Vector3.up, speed * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(objToCircle.position, Vector3.up, -speed * Time.deltaTime);
        }
    }

}
