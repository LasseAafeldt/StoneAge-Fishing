using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DelayPlayAudio : MonoBehaviour, IDelay
{
    private AudioSource audio;

    [SerializeField] private AudioClip clip;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        if (clip != null)
        {
            audio.clip = clip;
            audio.Play();
        }
    }
}
