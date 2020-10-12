using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidingArrowManager : MonoBehaviour
{
    IActivateArrow activator;
    IActivateAnimation animation;

    IPointArrowResponse arrowPointer;

    private void Awake()
    {
        arrowPointer = GetComponent<IPointArrowResponse>();
        if (arrowPointer == null)
            arrowPointer = GetComponentInChildren<IPointArrowResponse>();

        activator = GetComponent<IActivateArrow>();
        animation = GetComponent<IActivateAnimation>();
        if (animation != null)
        {
            animation.Setup(arrowPointer.GetGameObject());
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (activator.ShouldActivate())
        {
            if (!animation.IsActivated())
                animation.Activate(arrowPointer.GetGameObject());

            if (activator.IsInOppositeDirection())
                arrowPointer.PointHorinzontalTowards(activator.GetPointingTarget());
            else
                arrowPointer.PointTowards(activator.GetPointingTarget());
        }   
        else
        {
            if (animation.IsActivated())
            {
                animation.Deactivate(arrowPointer.GetGameObject());
            }
        }
    }
}
