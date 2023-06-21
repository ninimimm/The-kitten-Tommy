using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonKnife : MonoBehaviour
{
    [SerializeField] public float timeToPoison;
    [SerializeField] private float timeToWait;
    [SerializeField] private GameObject knifePrefab;
    private logicKnife _logicKnife;
    public float[] _timers;
    private float _timerWait;
    public IDamageable target;
    public int number;
    // Start is called before the first frame update
    void Start()
    {
        _logicKnife = knifePrefab.GetComponent<logicKnife>();
        _timers = new float[100000];
        for (var i = 0; i < _timers.Length; i++)
            _timers[i] = -0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < _timers.Length; i++)
        {
            if (_timers[i] >= 0) 
            {
                _timers[i] -= Time.deltaTime;
                _timerWait -= Time.deltaTime;
                if (_timerWait < 0)
                {
                    target.TakeDamage(_logicKnife.damage);
                    _timerWait = timeToWait;
                }
            }
        }
    }
}
