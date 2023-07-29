using System;
using System.Collections.Generic;
[Serializable]
public class SlimeData : IInitializable<Slime>
{
    public HashSet<string> start { get; set;} = new();
    public float HP;
    public int animatorState;
    public float[] position;
    
    public SlimeData()
    {
        position = new float[3];
    } 
    
    public void Initialize(Slime slime)
    {
        HP = slime.HP;
        animatorState = slime._animator.GetInteger("state");
        position[0] = slime.transform.position.x;
        position[1] = slime.transform.position.y;
        position[2] = slime.transform.position.z;
    } 
}