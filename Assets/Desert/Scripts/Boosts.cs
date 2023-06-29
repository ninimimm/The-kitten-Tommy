using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boosts : MonoBehaviour
{
    [SerializeField] private Image energy;
    [SerializeField] private Image[] boostsLite;
    [SerializeField] public TextMeshProUGUI[] boostsText;
    [SerializeField] private Image bottle;
    [SerializeField] private GameObject fish;
    [SerializeField] private float timeToSwitch;
    [SerializeField] private float timeToUse;
    [SerializeField] private float timeLite;
    [SerializeField] public float timeToRun;
    [SerializeField] public float timeToJump;
    [SerializeField] private float timeToShield;
    [SerializeField] private CatSprite cat;
    [SerializeField] private Shield shield;
    private BoostsData data;
    public int state;

    private float timer;

    private float timerChose = 0.01f;
    private int countLite;
    private float _runTimer = -0.1f;
    private float _jumpTimer = -0.1f;
    public int energyCount;
    public int waterCount;
    public int fishCount;
    private bool _canUse;
    private bool _cdNow;
    private float currentJump;
    // Start is called before the first frame update
    void Start()
    {
        data = SavingSystem<Boosts, BoostsData>.Load($"{gameObject.name}.data");
        currentJump = cat.jumpForce;
        timerChose = timeLite;
        if (!data.start.Contains(gameObject.name) && !MainMenu.isResume)
        {
            Save();
            data.start.Add(gameObject.name);
        }
        Load();
        boostsText[0].text = $"x{energyCount}";
        boostsText[1].text = $"x{fishCount}";
        boostsText[2].text = $"x{waterCount}";
    }
    public void Save()
    {
        SavingSystem<Boosts,BoostsData>.Save(this, $"{gameObject.name}.data");
    }


    public void Load()
    {
        data = SavingSystem<Boosts, BoostsData>.Load($"{gameObject.name}.data");
        energyCount = data.energyCount;
        fishCount = data.fishCount;
        waterCount = data.waterCount;
        timeToJump = data.timeToJump;
        timeToRun = data.timeToRun;
    }

    // Update is called once per frame
    void Update()
    {
        if (_runTimer >= 0) _runTimer -= Time.deltaTime;
        else cat.speed = 4f;
        if (_jumpTimer >= 0) _jumpTimer -= Time.deltaTime;
        else cat.jumpForce = currentJump;
        if (Input.GetKey(KeyCode.Q) && !_cdNow)
            timer += Time.deltaTime;
        if (!_cdNow)
        {
            if ((state == 0 && energyCount > 0) || (state == 1 && fishCount > 0) || (state == 2 && waterCount > 0))
                _canUse = true;
            else _canUse = false;
        }
        if (Input.GetKeyUp(KeyCode.Q) && timer < timeToSwitch)
        {
            boostsLite[state].enabled = false;
            boostsText[state].enabled = false;
            state = (state+1)%3;
            boostsLite[state].enabled = true;
            boostsText[state].enabled = true;
            timer = 0;
        }
        else if (timer >= timeToSwitch && Input.GetKeyUp(KeyCode.Q) && !_canUse) timer = 0;
        if (countLite < 4 && timer >= timeToSwitch && _canUse)
        {
            if (!_cdNow)
            {
                if (state == 0)
                {
                    cat.speed = 6f;
                    _runTimer = timeToRun;
                    energyCount--;
                    boostsText[state].text = "x" + energyCount;
                }
                if (state == 2)
                {
                    if (SceneManager.GetActiveScene().name == "ThirdLevle") cat.jumpForce *= 1.1f;
                    else cat.jumpForce *= 1.55f;
                    _jumpTimer = timeToJump;
                    waterCount--;
                    boostsText[state].text = "x" + waterCount;
                }
                if (state == 1)
                {
                    shield.timer = timeToShield;
                    fishCount--;
                    boostsText[state].text = "x" + fishCount;
                }
                _cdNow = true;
            }
            
            if (timerChose >= 0)
            {
                timerChose -= Time.deltaTime;
                if (timerChose < timeLite / 2)
                {
                    boostsLite[state].enabled = true;
                    boostsText[state].enabled = true;
                }
            }
            else
            {
                countLite++;
                boostsLite[state].enabled = false;
                boostsText[state].enabled = false;
                timerChose = timeLite;
            }
        }
        else if (countLite == 4)
        {
            countLite = 0;
            timer = 0;
            _cdNow = false;
            boostsLite[state].enabled = true;
            boostsText[state].enabled = true;
        }
    }
}