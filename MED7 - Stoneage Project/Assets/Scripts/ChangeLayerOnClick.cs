using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLayerOnClick : MonoBehaviour
{
    [SerializeField] private int maxClicks = 1;

    private int currentClicks = 0;

    public void  CountUp()
    {
        currentClicks++;
        if (currentClicks >=maxClicks)
        {

        }
    }

    private void ChangeLayer()
    {
        gameObject.layer = "default";
    }


}
