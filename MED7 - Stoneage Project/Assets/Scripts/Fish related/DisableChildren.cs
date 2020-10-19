using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableChildren : MonoBehaviour
{
    [SerializeField] private AcitivateFish typeInChildren;
    [SerializeField] private int fractionToRemovePerFishCaught = 4;
    private int amountOfChildren;
    int amountToRemove;
    private AcitivateFish[] acitivateFish;
    private Queue<AcitivateFish> queChildren = new Queue<AcitivateFish>();


    private void Start()
    {
        acitivateFish = GetComponentsInChildren<AcitivateFish>();
        amountOfChildren = acitivateFish.Length;
        foreach (AcitivateFish child in acitivateFish)
        {
            queChildren.Enqueue(child);
        }
        PartnerAnimator.FishPutInBasketEvent += OnFishCaught;
        Debug.Log("current fish: " + amountOfChildren);
        amountToRemove = Mathf.FloorToInt(amountOfChildren / fractionToRemovePerFishCaught);
    }

    private void DisableFish()
    {

        for (int i = 0; i < amountToRemove; i++)
        {
            if (queChildren.Count == 0)
                return;
            AcitivateFish child = queChildren.Dequeue(); //removes and returns object
            child.gameObject.SetActive(false);
        }
        Debug.Log("current fish: " + queChildren.Count);
    }

    private void OnFishCaught()
    {
        //make sure only to disable fish in the area the player is in
        if (gameObject.tag.Equals(EventCatcher.fishingArea))
        {
            DisableFish();
        }
    }

    private void OnDisable()
    {
        PartnerAnimator.FishPutInBasketEvent -= OnFishCaught;

    }
}
