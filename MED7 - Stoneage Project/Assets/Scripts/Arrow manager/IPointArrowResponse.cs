using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPointArrowResponse
{
    void PointTowards(Transform t);
    void PointHorinzontalTowards(Transform t);
    GameObject GetGameObject();
}
