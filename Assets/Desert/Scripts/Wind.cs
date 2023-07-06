using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attack;
    [SerializeField] private float timeToStan;
    [SerializeField] private LayerMask realEnemiesLayer;
    private SpriteRenderer sp;
    public float timer;
    private bool start = true;
    public bool isRight;
    private Collider2D[] hitEnemies = new Collider2D[0];
    private bool isCamp;

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        timer = -100000f;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            foreach (var enemy in hitEnemies)
                enemy.GetComponent<IDamageable>()?.TakeDamage(0, true); 
            timer -= Time.deltaTime;
        }
        else if (timer > -10)
        {
            foreach (var enemy in hitEnemies)
                enemy.GetComponent<IDamageable>()?.TakeDamage(0, false);
            Destroy(gameObject);
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("wind1"))
        {
            if (start)
            {
                if (isRight)
                    transform.position += new Vector3(0.5f, -0.1f, 0);
                else
                    transform.position += new Vector3(-0.5f, -0.1f, 0);
                start = false;
            }

            if (isRight) transform.position += new Vector3(speed, 0, 0);
            else transform.position += new Vector3(-speed, 0, 0);
        }
        if (Physics2D.OverlapCircle(attack.position, 0.1f, enemyLayer))
        {
            _animator.SetInteger("state",2);
            if (!isCamp)
            {
                isCamp = true;
                Сamp();
            }
        }
    }

    public void Сamp()
    {
        sp.enabled = false;
        hitEnemies = Physics2D.OverlapCircleAll(attack.position, 0.1f, realEnemiesLayer);
        if (hitEnemies.Length > 0) timer = timeToStan;
        foreach (var enemy in hitEnemies)
            enemy.GetComponent<IDamageable>()?.TakeDamage(0, true);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attack.position,0.1f);
    }
}
