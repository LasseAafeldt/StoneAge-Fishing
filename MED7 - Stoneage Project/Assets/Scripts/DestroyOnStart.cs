using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnStart : MonoBehaviour
{
    [SerializeField] private string NameOfObject; 
    void Awakw()
    {
        Destroy(GameObject.Find(NameOfObject));
    }
}
