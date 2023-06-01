using System;
using System.Collections.Generic;

[Serializable]
public class SnakeData : IInitializable<ComponentSnake>
{
    public static HashSet<string> start = new ();
    public float HP;
    public bool polyEnabled;
    public bool capEnabled;
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
        polyEnabled = snake.poly.enabled;
        capEnabled = snake.cap.enabled;
        animatorState = snake._animator.GetInteger("state");
        position[0] = snake.transform.position.x;
        position[1] = snake.transform.position.y;
        position[2] = snake.transform.position.z;
    } 
}