using System;
using System.Collections.Generic;

[Serializable]

public class DoorData : IInitializable<Door>
{
    public HashSet<string> start { get; set;} = new();
    public int animatorState;
    public bool colliderState;
    
    public DoorData() { } 
    
    public void Initialize(Door door)
    {
        animatorState = door.animator.GetInteger("state");
        colliderState = door.boxCollider2D.enabled;
    } 
}