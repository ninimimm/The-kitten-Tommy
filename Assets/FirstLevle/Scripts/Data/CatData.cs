
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class CatData : IInitializable<CatSprite>
{
    public static HashSet<string> start = new ();
    public float HP;
    public int countHealth;
    public int takeDamage;
    public string key;
    public int money;
    public float[] spawnFirstLevel;
    public float[] spawnSecondLevel;

    public CatData()
    {
        spawnFirstLevel = new float[3];
        spawnSecondLevel = new []{0,4.4f,0};
    } 
 
    public void Initialize(CatSprite catSprite)
    {
        var cat = catSprite.gameObject;
        HP = catSprite.HP;
        countHealth = catSprite.countHealth;
        takeDamage = catSprite.takeDamage;
        key = catSprite.key;
        money = catSprite.money;
        if (SceneManager.GetActiveScene().name == "FirstLevle")
        {
            spawnFirstLevel[0] = cat.transform.position.x-1;
            spawnFirstLevel[1] = cat.transform.position.y;
            spawnFirstLevel[2] = cat.transform.position.z;
        }
        else
        {
            spawnSecondLevel[0] = cat.transform.position.x;
            spawnSecondLevel[1] = cat.transform.position.y;
            spawnSecondLevel[2] = cat.transform.position.z;
        }
    } 
}
