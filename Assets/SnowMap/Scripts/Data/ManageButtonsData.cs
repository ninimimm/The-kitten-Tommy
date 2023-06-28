using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class ManageButtonsData : IInitializable<ManageButtons>
{
    public static HashSet<string> Start = new ();
    public bool colliderState;
    public bool animatorState;
    
    public ManageButtonsData(){} 
    
    public void Initialize(ManageButtons manageButtons)
    {
        animatorState = manageButtons.door.GetComponent<Animator>().GetBool("opened");
        colliderState = manageButtons.door.GetComponent<BoxCollider2D>().enabled;
    }
}