using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointArrowInCameraPlane : MonoBehaviour, IPointArrowResponse
{
    Transform camera;

    Vector3 arrow2target;

    [SerializeField] private float distanceToCam;

    private void Awake()
    {
        camera = Camera.main.transform;
        SetZPosition();
    }

    public void PointTowards(Transform t)
    {
        arrow2target =  t.position - transform.position;

        ProjectOntoCamPlane();

        transform.forward = arrow2target;
    }

   

    public void PointHorinzontalTowards(Transform t)
    {
        throw new System.NotImplementedException();
    }

    private void ProjectOntoCamPlane()
    {
        arrow2target = Vector3.ProjectOnPlane(arrow2target, camera.forward);
    }

    private void SetZPosition()
    {
        transform.localPosition = 
            new Vector3(transform.localPosition.x, 
            transform.localPosition.y, 
            distanceToCam);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
        
