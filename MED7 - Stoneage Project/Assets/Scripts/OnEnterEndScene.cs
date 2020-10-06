using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterEndScene : MonoBehaviour
{
    private AudioSource sound;

    private void Start()
    {
        sound = GameManager.singleton._audio;
        sound.Play();
    }
}
