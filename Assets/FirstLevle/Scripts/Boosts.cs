using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boosts : MonoBehaviour
{
    [SerializeField] private Image energy;
    [SerializeField] private Image[] bostsLite;
    [SerializeField] private Image bottle;
    [SerializeField] private GameObject fish;

    [SerializeField] private float timeToSwitch;
    [SerializeField] private float timeToUse;
    [SerializeField] private float timeLite;
    public int state;

    private float timer;

    private float timerChose = 0.01f;
    private int countLite;
    // Start is called before the first frame update
    void Start()
    {
        timerChose = timeLite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            timer += Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.Q) && timer < timeToSwitch)
        {
            bostsLite[state].enabled = false;
            state = (state+1)%3;
            bostsLite[state].enabled = true;
            timer = 0;
        }
        if (countLite < 4 && timer >= timeToSwitch)
        {
            if (timerChose >= 0)
            {
                timerChose -= Time.deltaTime;
                if (timerChose < timeLite / 2)
                    bostsLite[state].enabled = true;
            }
            else
            {
                countLite++;
                bostsLite[state].enabled = false;
                timerChose = timeLite;
            }
        }
        else if (countLite == 4)
        {
            countLite = 0;
            timer = 0;
            bostsLite[state].enabled = true;
        }
    }
}
