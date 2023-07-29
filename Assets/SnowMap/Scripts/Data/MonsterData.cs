using System;
using System.Collections.Generic;
[Serializable]
public class MonsterData : IInitializable<Monster>
{
    public HashSet<string> start { get; set;} = new();
    public float HP;
    public int animatorState;
    public float[] position;
    
    public MonsterData()
    {
        position = new float[3];
    } 
    
    public void Initialize(Monster monster)
    {
        HP = monster.HP;
        animatorState = monster.animator.GetInteger("state");
        position[0] = monster.transform.position.x;
        position[1] = monster.transform.position.y;
        position[2] = monster.transform.position.z;
    } 
}