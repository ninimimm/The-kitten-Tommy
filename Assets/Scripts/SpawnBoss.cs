using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private GameObject Cat;
    [SerializeField] private GameObject Mummy;
    [SerializeField] private GameObject Boss;
    [SerializeField] private GameObject Snake;
    //[SerializeField] private GameObject Scorpio;
    private Animator snakeAnim;

    private bool canSpawn = true;
    //private Animator scorpioAnim;

    private Animator[] Animators;
    // Start is called before the first frame update
    void Start()
    {
        snakeAnim = Snake.GetComponent<Animator>();
        //scorpioAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (snakeAnim.GetCurrentAnimatorStateInfo(0).IsName("death") && canSpawn)
        {
            canSpawn = false;
            GameObject boss = Instantiate(Boss, spawnPosition, Quaternion.identity);
            boss.GetComponent<SandBoss>()._cat = Cat;
            boss.GetComponent<SandBoss>().MummyPrefab = Mummy;
        }
            
    }
}
