using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonKnife : MonoBehaviour
{
    [SerializeField] public float timeToPoison;
    [SerializeField] private float timeToWait;
    [SerializeField] private GameObject knifePrefab;
    private logicKnife _logicKnife;
    public float _timer = -0.1f;
    private float _timerWait;
    public IDamageable target;
    // Start is called before the first frame update
    void Start()
    {
        _logicKnife = knifePrefab.GetComponent<logicKnife>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer >= 0)
        {
            _timer -= Time.deltaTime;
            _timerWait -= Time.deltaTime;
            if (_timerWait < 0)
            {
                target.TakeDamage(_logicKnife.damage);
                _timerWait = timeToWait;
            }
        }
    }
}
