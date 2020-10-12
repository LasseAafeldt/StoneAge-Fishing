using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour, IActivateAnimation
{
    [SerializeField] float duration;

    public bool isActivated = false;

    public void Setup(GameObject go)
    {
        go.SetActive(false);
    }

    public bool IsActivated()
    {
        return isActivated;
    }

    public void Activate(GameObject go)
    {
        go.SetActive(true);
        isActivated = true;
        ScaleUp(go);

    }

    public void Deactivate(GameObject go)
    {
        isActivated = false;
        ScaleDown(go);
    }

    private void ScaleUp(GameObject go)
    {
        go.transform.localScale = Vector3.zero;
        StartCoroutine(Scale(1, 0, duration, go, false));
    }
    private void ScaleDown(GameObject go)
    {
        go.transform.localScale = Vector3.one;
        StartCoroutine(Scale(0, 1, duration, go, true));
    }

    private IEnumerator Scale(float from, float to, float totalDuration, GameObject go, bool deactivateAfterwards)
    {
        float currentDuration = totalDuration;
        float currentScale;

        while (currentDuration > 0)
        {
            float t = currentDuration / totalDuration;
            currentScale = Mathf.SmoothStep(from, to, t);
            go.transform.localScale = Vector3.one * currentScale; 
            currentDuration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (deactivateAfterwards)
            go.SetActive(false);
    }

}
