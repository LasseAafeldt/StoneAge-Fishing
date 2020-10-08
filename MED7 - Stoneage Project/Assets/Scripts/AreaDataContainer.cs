using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AreaDataContainer
{
    public string name;
    public GameObject gObject;
    public Vector3 position;
    public float distanceFromPlayer;
    public float horizontalAngleFromLookDirection;
    public float maxDistFromPlayer;
    public float maxAngleFromPlayer;
    public float scoreDistWeight;
    public float scoreAngleWeight;

    public AreaDataContainer(string Mname, GameObject gameObj, Vector3 Mposition, float MdistanceFromPlayer, 
        float MhorizontalAngleFromLookDirection, float MmaxDistToPlayer, float MmaxAngleFromPlayer,
        float MscoreDistWeight, float MscoreAngleWeight)
    {
        name = Mname;
        gObject = gameObj;
        position = Mposition;
        distanceFromPlayer = MdistanceFromPlayer;
        horizontalAngleFromLookDirection = MhorizontalAngleFromLookDirection;
        maxDistFromPlayer = MmaxDistToPlayer;
        maxAngleFromPlayer = MmaxAngleFromPlayer;
        scoreDistWeight = MscoreDistWeight;
        scoreAngleWeight = MscoreAngleWeight;
    }

    public float GetScore()
    {
        float angleScore = maxAngleFromPlayer / maxAngleFromPlayer;
        float distScore = distanceFromPlayer / maxDistFromPlayer;

        float totalScore = angleScore * scoreAngleWeight + distScore * scoreDistWeight;
        return totalScore;
    }

}
