using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicKnife : MonoBehaviour
{
    [SerializeField] public float speed = 10f;
    [SerializeField] public float damage = 1;
    private Rigidbody2D _rb;
    private LogicKnifeData data;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.right * speed;
        if (!LogicKnifeData.start.Contains(gameObject.name))
        {
            LogicKnifeData.start.Add(gameObject.name);
            Save();
        }
        Load();
    }
    
    public void Save()
    {
        SavingSystem<logicKnife,LogicKnifeData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        data = SavingSystem<logicKnife, LogicKnifeData>.Load($"{gameObject.name}.data");
        damage = data.damage;
    }

    // FixedUpdate is called at fixed intervals
    void FixedUpdate()
    {
        // Calculate the rotation angle based on the velocity
        float angle = Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg;

        // Set the knife's rotation
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle+30));
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        var enemy = hitInfo.GetComponent<IDamageable>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}