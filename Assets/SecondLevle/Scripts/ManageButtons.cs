using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ManageButtons : MonoBehaviour
{
    [SerializeField] public GameObject door;
    public StringBuilder keys = new ();
    public GameObject[] buttons;
    [SerializeField] private float timeToWait;
    public float timer;
    private ManageButtonsData _data;

    private void Start()
    {
        if (!ManageButtonsData.Start.Contains(gameObject.name))
        {
            ManageButtonsData.Start.Add(gameObject.name);
            Save();
        }
        Load();
    }

    void Update()
    {
        if (keys.Length == 5)
        {
            if (keys.ToString() == "41523")
            {
                door.GetComponent<Animator>().SetBool("opened",true);
                door.GetComponent<BoxCollider2D>().enabled = false;
            }
                
            else if (timer < -0.4)
                timer = timeToWait;
            }

        if (timer < 0 && timer > -0.2)
        {
            foreach (var button in buttons)
                button.GetComponent<Button>().state = Button.MovementState.Stay;
            keys = new StringBuilder("");
        }
        timer -= Time.deltaTime;
    }
    
    public void Save()
    {
        SavingSystem<ManageButtons,ManageButtonsData>.Save(this, $"{gameObject.name}.data");
    }


    public void Load()
    {
        _data = SavingSystem<ManageButtons, ManageButtonsData>.Load($"{gameObject.name}.data");
        door.GetComponent<Animator>().SetBool("opened",_data.animatorState);
        door.GetComponent<BoxCollider2D>().enabled = _data.colliderState;
    }
}
