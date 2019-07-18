using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour {
    Animator animator;

    private void Start()
    {
        try
        {
            animator = GetComponentInChildren<Animator>();
            //Debug.Log("anim = " + animator);
        }
        catch
        {
            Debug.LogError("Camera could not find an Animator in children");
        }
    }

    public void fadeOut()
    {
        animator.SetTrigger("FadeOut");
    }
}
