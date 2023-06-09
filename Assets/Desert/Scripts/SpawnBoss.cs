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
    [SerializeField] private AudioSource _audioBall;
    [SerializeField] private AudioSource _audioSourceBossHurt;
    [SerializeField] private AudioSource _audioSourceMummyHurt;
    [SerializeField] private AudioSource _audioSourceMummyAttack;
    [SerializeField] private Experience XP;

    private Animator snakeAnim;
    public bool canSpawn = true;

    private void Start()
    {
        snakeAnim = Snake.GetComponent<Animator>();
    }

    private void Update()
    {
        if (canSpawn && Cat.transform.position.x > 36)
        {
            canSpawn = false;
            SpawnBossInstance();
        }
    }

    private void SpawnBossInstance()
    {
        var newCanvas = Instantiate(canvasBoss, new Vector3(spawnPosition.x, spawnPosition.y + valueBoss, spawnPosition.z), Quaternion.identity);
        var healthBar = newCanvas.GetComponentInChildren<HealthBar>();
        var _fill = healthBar.GetComponentsInChildren<Image>()[0].GetComponent<Image>();
        var _bar = healthBar.GetComponentsInChildren<Image>()[1].GetComponent<Image>();

        var boss = Instantiate(Boss, spawnPosition, Quaternion.identity);
        GoToSecondLevle.boss = boss.GetComponent<SandBoss>();
        var sandBoss = boss.GetComponent<SandBoss>();
        sandBoss.XP = XP;
        sandBoss._cat = Cat;
        sandBoss.MummyPrefab = Mummy;
        sandBoss._healthBar = healthBar;
        sandBoss.fill = _fill;
        sandBoss.bar = _bar;
        sandBoss._canvasMummy = canvasMummy;
        sandBoss._valueMummy = valueMummy;
        sandBoss._audioBall = _audioBall;
        sandBoss._audioSourceBossHurt = _audioSourceBossHurt;
        sandBoss._audioSourceMummyAttack = _audioSourceMummyAttack;
        sandBoss._audioSourceMummyHurt = _audioSourceMummyHurt;

        healthBar.GetComponent<EnemyHealthBar>()._target = boss.transform;
        healthBar.GetComponent<EnemyHealthBar>()._value = valueBoss;
    }
}
