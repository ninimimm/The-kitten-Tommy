using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private GameObject Cat;
    [SerializeField] private GameObject Mummy;
    [SerializeField] private GameObject Boss;
    [SerializeField] private GameObject Snake;
    [SerializeField] private Canvas canvasBoss;
    [SerializeField] private Canvas canvasMummy;
    [SerializeField] private float valueBoss;
    [SerializeField] private float valueMummy;
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
        if (snakeAnim.GetCurrentAnimatorStateInfo(0).IsName("death") && canSpawn && Cat.transform.position.x > 36)
        {
            canSpawn = false;
            
            var newCanvas = Instantiate(canvasBoss,
                new Vector3(spawnPosition.x, spawnPosition.y + valueBoss, spawnPosition.z), Quaternion.identity);
            var healthBar = newCanvas.GetComponentInChildren<HealthBar>();
            var _fill = healthBar.GetComponentsInChildren<Image>()[0].GetComponent<Image>();
            var _bar = healthBar.GetComponentsInChildren<Image>()[1].GetComponent<Image>();
            GameObject boss = Instantiate(Boss, spawnPosition, Quaternion.identity);
            healthBar.GetComponent<EnemyHealthBar>()._target = boss.transform;
            healthBar.GetComponent<EnemyHealthBar>()._value = valueBoss;
            boss.GetComponent<SandBoss>()._cat = Cat;
            boss.GetComponent<SandBoss>().MummyPrefab = Mummy;
            boss.GetComponent<SandBoss>()._healthBar = healthBar;
            boss.GetComponent<SandBoss>().fill = _fill;
            boss.GetComponent<SandBoss>().bar = _bar;
            boss.GetComponent<SandBoss>()._canvasMummy = canvasMummy;
            boss.GetComponent<SandBoss>()._valueMummy = valueMummy;
        }
            
    }
}
