using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TakeKeyData : IInitializable<TakeKey>
{
    public static HashSet<string> start = new ();

    public bool goldKeyImageEnabled;
    public bool ironKeyImageEnabled;
    public int animatorGoldChestState;
    public int animatorIronChestState;
    public float[] positionGoldKey;
    public float[] positionIronKey;
    public bool goldIsNull;
    public bool ironIsNull;
    public bool isOpenedGoldChest;
    public bool isOpenedIronChest;
    
    public TakeKeyData()
    {
        positionGoldKey = new float[3];
        positionIronKey = new float[3];
        goldIsNull = true;
        ironIsNull = true;
    } 
    
    public void Initialize(TakeKey key)
    {
        goldKeyImageEnabled = key.goldKeyImage.enabled;
        ironKeyImageEnabled = key.ironKeyImage.enabled;
        animatorGoldChestState = key._animatorGoldChest.GetInteger("state");
        animatorIronChestState = key._animatorIronChest.GetInteger("state");
        if (!goldIsNull)
        {
            positionGoldKey[0] = key._goldKey.transform.position.x;
            positionGoldKey[1] = key._goldKey.transform.position.y;
            positionGoldKey[2] = key._goldKey.transform.position.z;
        }
        if (!ironIsNull)
        {
            positionIronKey[0] = key._ironKey.transform.position.x;
            positionIronKey[1] = key._ironKey.transform.position.y;
            positionIronKey[2] = key._ironKey.transform.position.z;
        }
        isOpenedGoldChest = key.isOpenedGoldChest;
        isOpenedIronChest = key.isOpenedIronChest;
    } 
}