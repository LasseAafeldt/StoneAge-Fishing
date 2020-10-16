using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableChildren : MonoBehaviour
{
    [SerializeField] private AcitivateFish typeInChildren;
    [SerializeField] private int fractionToRemovePerFishCaught = 3;
    private int amountOfChildren;
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
    }

    private void DisableFish(int fractionToRemove)
    {
        //calculate a third
        int amountToRemove = Mathf.FloorToInt(amountOfChildren / fractionToRemove);
        for (int i = 0; i < amountToRemove; i++)
        {
            if (queChildren.Count == 0)
                return;
            AcitivateFish child = queChildren.Dequeue(); //removes and returns object
            child.gameObject.SetActive(false);
        }
    }

    private void OnFishCaught()
    {
        //make sure only to disable fish in the area the player is in
        if (gameObject.tag.Equals(EventCatcher.fishingArea))
        {
            DisableFish(fractionToRemovePerFishCaught);
        }
    }
}
