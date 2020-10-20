using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeController : MonoBehaviour {

    CanvasGroup canvas;
    [SerializeField] private float animationTime=2;

    private void Start()
    {
        try
        {
            canvas = GetComponentInChildren<CanvasGroup>();
        }
        catch
        {
            Debug.LogError("Camera could not find an Canvasgroup in children");
        }

        InstantFadeOut();
        fadeIn();
    }

    public void fadeOut()
    {
        Debug.Log("alpha: " + canvas.alpha);
        if (canvas.alpha <= .1)
        {
            StartCoroutine(InterpolateAlpha(1, 0, animationTime));
        }

    }

    public void fadeIn()
    {
        if (canvas.alpha !=0)
        {
            StartCoroutine(InterpolateAlpha(0, 1, animationTime));
        }

    }

    public void InstantFadeOut()
    {
        canvas.alpha = 1;
    }

    public void InstantFadeIn()
    {
        canvas.alpha = 0;
    }

    private IEnumerator InterpolateAlpha(float from, float to, float totalDuration)
    {
        float currentDuration = totalDuration;

        while (currentDuration > 0)
        {
            float t = currentDuration / totalDuration;
            canvas.alpha = Mathf.SmoothStep(from, to, t);
            currentDuration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}