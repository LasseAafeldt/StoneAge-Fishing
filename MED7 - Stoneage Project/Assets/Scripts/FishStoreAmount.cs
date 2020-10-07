using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStoreAmount : MonoBehaviour
{
    [SerializeField] private int _eelAmount;
    [SerializeField] private int _flatfishAmount;
    [SerializeField] private int _torskAmount;

    public int EelAmount { get { return _eelAmount; } }
    public int FlatfishlAmount { get { return _flatfishAmount; } }
    public int TorskAmount { get { return _torskAmount; } }

}
