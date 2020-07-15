using UnityEngine;

public class FadeController : MonoBehaviour {

    Animator animator;

    private void Start()
    {
        try
        {
            animator = GetComponentInChildren<Animator>();
        }
        catch
        {
            Debug.LogError("Camera could not find an Animator in children");
        }
    }

    public void fadeOut()
    {
        animator?.SetTrigger("FadeOut");
    }

    public void fadeIn()
    {
        animator?.SetTrigger("FadeIn");
    }

    public void InstantFadeOut()
    {
        animator?.SetTrigger("InstantFadeOut");
    }

    public void InstantFadeIn()
    {
        animator?.SetTrigger("InstantFadeIn");
    }
}