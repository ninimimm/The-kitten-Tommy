using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[Serializable]
public class CatData : IInitializable<CatSprite>
{
    public HashSet<string> start { get; set;} = new();
    public float HP;
    public float XP;
    public int countHealth;
    public int takeDamage;
    public string key;
    public int money;
    public float[] spawnFirstLevel;
    public float[] spawnSecondLevel;
    public float[] spawnThirdLevel;
    public bool isPoison;
    public bool canSpawn;

    public CatData()
    {
        spawnFirstLevel = new float[3];
        spawnSecondLevel = new []{0,4.4f,0};
        spawnThirdLevel = new []{0,4.6f,0};
        
    } 
 
    public void Initialize(CatSprite catSprite)
    {
        var cat = catSprite.gameObject;
        HP = catSprite.HP;
        countHealth = catSprite.countHealth;
        takeDamage = catSprite.takeDamage;
        key = catSprite.key;
        money = catSprite.money;
        isPoison = catSprite.isPoison;
        if (SceneManager.GetActiveScene().name == "FirstLevle")
        {
            spawnFirstLevel[0] = cat.transform.position.x-1;
            spawnFirstLevel[1] = cat.transform.position.y;
            spawnFirstLevel[2] = cat.transform.position.z;
        }
        else if(SceneManager.GetActiveScene().name == "SecondLevle")
        {
            spawnSecondLevel[0] = cat.transform.position.x;
            spawnSecondLevel[1] = cat.transform.position.y;
            spawnSecondLevel[2] = cat.transform.position.z;
        }
        else
        {
            spawnThirdLevel[0] = cat.transform.position.x;
            spawnThirdLevel[1] = cat.transform.position.y;
            spawnThirdLevel[2] = cat.transform.position.z;
        }
        canSpawn = catSprite.canSpawn;
        XP = catSprite.XP;
    } 
}
