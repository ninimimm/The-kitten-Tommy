using System;
using System.Collections.Generic;
[Serializable]
public class HawkData : IInitializable<Hawk>
{
    public HashSet<string> start { get; set;} = new();
    public float HP;
    public bool capsFirstEnabled;
    public bool capsSecondEnabled;
    public int animatorState;
    public float[] position;
    
    public HawkData()
    {
        position = new float[3];
    } 
    
    public void Initialize(Hawk hawk)
    {
        var obj = hawk.gameObject;
        HP = hawk.HP;
        capsFirstEnabled = hawk.caps[0].enabled;
        capsSecondEnabled = hawk.caps[1].enabled;
        animatorState = hawk._animator.GetInteger("state");
        position[0] = hawk.transform.position.x;
        position[1] = hawk.transform.position.y;
        position[2] = hawk.transform.position.z;
    } 
}