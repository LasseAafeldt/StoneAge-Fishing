using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivateAnimation 
{
    void Activate(GameObject go);
    void Deactivate(GameObject go);
    bool IsActivated();
    void Setup(GameObject go);
}
