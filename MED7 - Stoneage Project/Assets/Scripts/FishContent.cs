using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishContent : MonoBehaviour {

	public List<GameObject> fish = new List<GameObject>();

    PartnerSpeech ps;

    bool areaOutOfFish;

    private void Start()
    {
        areaOutOfFish = false;
        ps = GameManager.singleton.partner.GetComponent<PartnerSpeech>();
    }

    public void RemoveFish()
	{
		if(fish.Count >0)
		{
			//Debug.Break();
			fish[0].gameObject.SetActive(false);
			fish.RemoveAt(0);
			Debug.Log("number of fish in area " +fish.Count);
		}
        else if(fish.Count <= 0)
        {
            Debug.Log("number of fish in area " + fish.Count);
            areaOutOfFish = true;
            ps.PartnerSaysSomething(ps.areaIsOutOfFish);
        }

	}
	/*public void DestroyEmptyArea()
	{
		if(fish.Count == 0)
		{
			Debug.Log("destroying the area");
            //Destroy(gameObject);
            gameObject.SetActive(false);
			GameManager.singleton.boat.GetComponent<EventCatcher>().ExitArea();
		}
	}*/

    public bool getAreaOutOfFish()
    {
        return areaOutOfFish;
    }
}
