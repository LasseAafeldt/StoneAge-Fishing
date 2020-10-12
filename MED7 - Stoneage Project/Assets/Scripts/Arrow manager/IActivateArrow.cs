using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivateArrow
{
    bool ShouldActivate();
    bool IsInOppositeDirection();

    Transform GetPointingTarget();

}
