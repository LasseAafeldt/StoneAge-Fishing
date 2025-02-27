﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcitivateFish : MonoBehaviour {
    public GameObject childObj;
    [ColorUsageAttribute(false, true)]
    public Color startEmissionColor;
    public float fadeSpeed = 1f;
    private Color emmisionColor;
    private Renderer fishRendere;
    private Material fishMat;
    float intensity = 0;
    private bool fadeIn = false;
    private bool fadeOut = false;
    
    float amplitude = 1; //should make it so that it's unchanged from editor value

    private void Start()
    {
        fishRendere = childObj.GetComponent<globalFlock>().getTorskRendere();
        fishMat = childObj.GetComponent<globalFlock>().getTorskRendere().sharedMaterial;
        emmisionColor = startEmissionColor;
        fadeIn = false;
        fadeOut = false;
        activateChild();
}

    private void Update()
    {
        if(fadeIn && intensity <= 1)
        {
            intensity += Time.deltaTime * fadeSpeed;
            if (intensity > 1)
                fadeIn = false;
        }
        if(fadeOut && intensity >= 0)
        {
            intensity -= Time.deltaTime * fadeSpeed;
            if (intensity < 0)
                fadeOut = false;
        }
        if(fadeIn || fadeOut)
        {
            setEmission();
        }
    }

    public void activateChild()
    {
        childObj.SetActive(true);
        //do some animation fade in stuff
        fishFadeIn();
    }

    public void deactivateChild()
    {
        //do some animation fade out stuff
        fishFadeOut();
        StartCoroutine(fishDeactivationDelay());
    }

    private void fishFadeIn()
    {
        fadeIn = true;
    }

    private void fishFadeOut()
    {
        fadeOut = true;
    }

    private void setEmission()
    {
        fishMat.SetColor("_EmissionColor", emmisionColor * intensity);
    }

    IEnumerator fishDeactivationDelay()
    {
        float waitTime = 1/fadeSpeed;
        yield return new WaitForSeconds(waitTime);
        childObj.SetActive(false);
    }
}
