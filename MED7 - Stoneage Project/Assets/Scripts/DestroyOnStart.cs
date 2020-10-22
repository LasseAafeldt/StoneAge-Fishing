using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnStart : MonoBehaviour
{
    [SerializeField] private string NameOfObject; 
    void Start()
    {
        Destroy(GameObject.Find(NameOfObject));
        //Objects[] objects = findg
        //if (gms.Length > 0)
        //{
        //    foreach (GameManager gm in gms)
        //    {
        //        Debug.Log("Destroying " + gm.name);
        //        Destroy(gm.gameObject);
        //    }   

    }
}
