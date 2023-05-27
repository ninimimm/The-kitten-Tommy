using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private CatSprite catSprite;
    [SerializeField] public float attackIntervale;
    public Transform fireKnife;
    public GameObject knife;
    public float timer;
    private bool nowIsShoot = false;
    private KnifeData data;


    private void Start()
    {
        if (!KnifeData.start.Contains(gameObject.name))
        {
            KnifeData.start.Add(gameObject.name);
            Save();
        }
        Load();
        timer = attackIntervale;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > attackIntervale)
            nowIsShoot = false;
        if (nowIsShoot) timer += Time.deltaTime;
        else if (Input.GetButtonDown("Fire1"))
        {
            timer = 0;
            Shoot();
            nowIsShoot = true;
        }
    }
    
    public void Save()
    {
        SavingSystem<Knife,KnifeData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        data = SavingSystem<Knife, KnifeData>.Load($"{gameObject.name}.data");
        attackIntervale = data.attackIntervale;
        catSprite._knifeBar.SetMaxHealth(attackIntervale);
    }

    void Shoot()
    {
        GameObject newKnife = Instantiate(knife, fireKnife.position, fireKnife.rotation);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector2 direction = (new Vector2(mouseWorldPosition.x, mouseWorldPosition.y) - (Vector2)fireKnife.position).normalized;
        newKnife.GetComponent<Rigidbody2D>().velocity = direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newKnife.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
