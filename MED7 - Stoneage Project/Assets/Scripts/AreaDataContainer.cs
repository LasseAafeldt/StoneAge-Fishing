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
    public float guidanceScore {
        get {
            float tempDistScore;
            if (distScore >= 1)
            {
                tempDistScore = distScore * 1.1f;
            }

            float totalScore = angleScore * scoreAngleWeight + distScore * scoreDistWeight;
            return totalScore;
        }
        private set { }
    }
    public float angleScore {
        get
        {
            return horizontalAngleFromLookDirection / 90;
        }
    }
    public float distScore {
        get
        {
            return distanceFromPlayer / maxDistFromPlayer;
        }
    }
        

    public AreaDataContainer(string Mname, GameObject MgameObj, float MmaxDistFromPlayer, float MmaxAngleFromPlayer,
        float MscoreDistWeight, float MscoreAngleWeight)
    {
        name = Mname;
        gObject = MgameObj;
        position = MgameObj.transform.position;
        distanceFromPlayer = 0f;
        horizontalAngleFromLookDirection = 0f;
        maxDistFromPlayer = MmaxDistFromPlayer;
        maxAngleFromPlayer = MmaxAngleFromPlayer;
        scoreDistWeight = MscoreDistWeight;
        scoreAngleWeight = MscoreAngleWeight;
        guidanceScore = 0f;
    }

    //public float GetScore()
    //{
    //    float angleScore = maxAngleFromPlayer / maxAngleFromPlayer;
    //    float distScore = distanceFromPlayer / maxDistFromPlayer;
    //    if(distScore >= 1)
    //    {
    //        distScore *= 1.5f;
    //    }

    //    float totalScore = angleScore * scoreAngleWeight + distScore * scoreDistWeight;
    //    return totalScore;
    //}

}
