using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageFishPosition : MonoBehaviour {

    public List<AcitivateFish> fishContainers = new List<AcitivateFish>();

    public Collider camContainerCollider;

    private void Start()
    {
        //set the position to an everage of all the fish spawner objects' position the belong to this area.
        transform.position = averagePosition();
    }

    Vector3 averagePosition()
    {
        if (fishContainers.Count == 0)
            return Vector3.zero;
        float x = 0f;
        float y = 0f;
        float z = 0f;
        foreach (AcitivateFish _fishSpawner in fishContainers)
        {
            Vector3 pos = _fishSpawner.transform.position;
            x += pos.x;
            y += pos.y;
            z += pos.z;
        }
        return new Vector3(x / fishContainers.Count, y / fishContainers.Count, z / fishContainers.Count);
    }
}
