using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class DoorData : IInitializable<Door>
{
    public static HashSet<string> Start = new ();
    public int animatorState;
    public bool colliderState;
    
    public DoorData() { } 
    
    public void Initialize(Door door)
    {
        animatorState = door.animator.GetInteger("state");
        colliderState = door.boxCollider2D.enabled;
    } 
}