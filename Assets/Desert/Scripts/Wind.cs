using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attack;
    private bool start = true;
    public bool isRight;

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }

    public void Сamp()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attack.position,0.1f);
    }
}
