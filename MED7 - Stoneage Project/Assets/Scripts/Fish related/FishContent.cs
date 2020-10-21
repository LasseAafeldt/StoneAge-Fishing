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
			fish[0].gameObject.SetActive(false);
			fish.RemoveAt(0);
		}
        else if(fish.Count <= 0)
        {
            areaOutOfFish = true;
            ps.PartnerSaysSomething(ps.areaIsOutOfFish);
        }

	}

    public bool getAreaOutOfFish()
    {
        return areaOutOfFish;
    }
}
