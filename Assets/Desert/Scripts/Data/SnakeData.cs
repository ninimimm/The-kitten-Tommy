using System;
using System.Collections.Generic;

[Serializable]
public class SnakeData : IInitializable<ComponentSnake>
{
    public HashSet<string> start { get; set;} = new();
    public float HP;
    public bool cap1Enabled;
    public bool cap2Enabled;
    public bool cap3Enabled;
    public int animatorState;
    public float[] position;
    
    public SnakeData()
    {
        position = new float[3];
    } 
    
    public void Initialize(ComponentSnake snake)
    {
        var obj = snake.gameObject;
        HP = snake.HP;
        cap1Enabled = snake.cap1.enabled;
        cap2Enabled = snake.cap2.enabled;
        cap3Enabled = snake.cap3.enabled;
        animatorState = snake._animator.GetInteger("state");
        position[0] = snake.transform.position.x;
        position[1] = snake.transform.position.y;
        position[2] = snake.transform.position.z;
    } 
}