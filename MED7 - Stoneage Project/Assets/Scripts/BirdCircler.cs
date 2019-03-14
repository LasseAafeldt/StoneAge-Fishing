using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCircler : MonoBehaviour {

    public Transform objToCircle;
    public float speed = 15;

    private void FixedUpdate()
    {
        transform.RotateAround(objToCircle.position, Vector3.up, speed * Time.deltaTime);
    }

}
