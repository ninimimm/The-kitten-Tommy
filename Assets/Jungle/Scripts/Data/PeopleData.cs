using System;
using System.Collections.Generic;
[Serializable]
public class PeopleData : IInitializable<People>
{
    public static HashSet<string> start = new ();
    public float HP;
    public bool polyEnabled;
    public bool boxEnabled;
    public int animatorState;
    public float[] position;
    
    public PeopleData()
    {
        position = new float[3];
    } 
    
    public void Initialize(People people)
    {
        var obj = people.gameObject;
        HP = people.HP;
        polyEnabled = people._poly.enabled;
        boxEnabled = people._boxCol.enabled;
        animatorState = people.animator.GetInteger("state");
        position[0] = people.transform.position.x;
        position[1] = people.transform.position.y;
        position[2] = people.transform.position.z;
    } 
}