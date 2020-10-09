using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHorizontalDirection : MonoBehaviour
{
    public Transform matchObject;

    // Start is called before the first frame update
    void Start()
    {
        if (matchObject)
        {
            SetRotation(matchObject);
        }
    }

    void SetRotation(Transform target)
    {
        Vector3 degrees = target.eulerAngles;

        Vector3 onlyY = new Vector3(0, degrees.y, 0);

        transform.eulerAngles = onlyY;
    }
}
