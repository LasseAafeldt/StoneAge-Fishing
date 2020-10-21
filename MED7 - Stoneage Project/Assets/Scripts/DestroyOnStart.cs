using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnStart : MonoBehaviour
{
    [SerializeField] private string nameOfTagToDestroy;
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("nameOfTagToDestroy");
        if (objs.Length > 0)
        {
            foreach (GameObject obj in objs)
            {
                Debug.Log("Destroying " + nameOfTagToDestroy);
                Destroy(obj);
            }
        }
#if UNITY_EDITOR
        Debug.LogError("Tag you are trying to find does not exist ", gameObject);         
#endif
    }
}
