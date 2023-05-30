using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private CatSprite _cat;
    [SerializeField] private float timeFire;
    [SerializeField] private float timeWait;
    [SerializeField] private GameObject[] fires;

    [SerializeField] private GameObject logPrefab;
    [SerializeField] private float damageLog;
    [SerializeField] private float logForce;
    [SerializeField] private float minTimeLog;
    [SerializeField] private float maxTimeLog;
    [SerializeField] private float minTimeStone;
    [SerializeField] private float maxTimeStone;
    [SerializeField] private float minSpawnX;
    [SerializeField] private float maxSpawnX;
    [SerializeField] private float destroyX;
    [SerializeField] private Stone[] stones;
    [SerializeField] private float timeRain;
    [SerializeField] private float timeMiddle;
    [SerializeField] private float timeHole;
    [SerializeField] private float timeStairs;
    [SerializeField] private float timeCentre;
    private bool isFireOn = true;
    public List<Log> logs = new List<Log>();
    private int _position;
    private SpriteRenderer[] _spriteRenderers;
    private PolygonCollider2D[] _polygonColliders;
    private float _rainTimer = -0.1f;
    private float _middleTimer = -0.1f;
    private float _holeTimer = -0.1f;
    private float _stairsTimer = -0.1f;
    private float _centreTimer = -0.1f;
    private bool _isRain;
    private bool _isMiddle;
    private bool _isHole;
    private bool _isStairs;
    private bool _isCentre;
    private List<Rigidbody2D> _stoneBodies = new ();

    void Start()
    {
        foreach (var stone in stones)
            _stoneBodies.Add(stone.GetComponent<Rigidbody2D>());
        _spriteRenderers = new SpriteRenderer[fires.Length];
        _polygonColliders = new PolygonCollider2D[fires.Length];
        for (var i = 0; i < fires.Length; i++)
        {
            _polygonColliders[i] = fires[i].GetComponent<PolygonCollider2D>();
            _spriteRenderers[i] = fires[i].GetComponent<SpriteRenderer>();
        }

        StartCoroutine(FireRoutine());
        StartCoroutine(LogRoutine());
        StartCoroutine(NewStoneAttack());
    }

    void FixedUpdate()
    {
        for (var i = logs.Count - 1; i >= 0; i--)
        {
            if (logs[i].gameObject.transform.position.x <= destroyX)
            {
                Log logToDestroy = logs[i];
                logs.RemoveAt(i);
                Destroy(logToDestroy.gameObject);
            }
        }
        if (_isRain) StoneRain();
        if (_isMiddle) MiddleAfterOther();
        if (_isHole) HoledRain();
        if (_isStairs) StairsRain();
        if (_isCentre) ToCentreRain();
    }

    public void LogDestroyed(Log log)
    {
        logs.Remove(log);
        Destroy(log.gameObject);
    }

    
    IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(isFireOn ? timeFire : timeWait);
            isFireOn = !isFireOn;
            ToggleFire(isFireOn);
        }
    }

    IEnumerator LogRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeLog, maxTimeLog));

            var newLogObj = Instantiate(logPrefab, new Vector3(Random.Range(minSpawnX, maxSpawnX),
                logPrefab.transform.position.y, logPrefab.transform.position.z), Quaternion.identity);
            var newLog = newLogObj.GetComponent<Log>();
            newLog._cat = _cat;
            newLog._boss = gameObject;
            newLog.damage = damageLog;
            var rb = newLogObj.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(-logForce, 0));
            logs.Add(newLog);
        }
    }
    
    IEnumerator NewStoneAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeStone, maxTimeStone));
            switch (Random.Range(0, 5))
            {
                case 0:
                    _isRain = true;
                    break;
                case 1:
                    _isMiddle = true;
                    break;
                case 2:
                    _isHole = true;
                    break;
                case 3:
                    _isCentre = true;
                    break;
                case 4:
                    _isStairs = true;
                    break;
            }
        }
    }
    
    void ToggleFire(bool fireState)
    {
        for (var i = 0; i < fires.Length; i++)
        {
            _polygonColliders[i].enabled = fireState;
            fires[i].layer = fireState ? 18 : 0;
            _spriteRenderers[i].enabled = fireState;
        }
    }

    void StoneRain()
    {
        _rainTimer -= Time.deltaTime;
        if (_rainTimer < 0)
        {
            _stoneBodies[_position].bodyType = RigidbodyType2D.Dynamic;
            _position++;
            if (_position == 7)
            {
                _position = 0;
                _isRain = false;
            }
            _rainTimer = timeRain;
        }
    }

    void HoledRain()
    {
        _holeTimer -= Time.deltaTime;
        if (_holeTimer < 0)
        {
            if (_position == 0)
            {
                _stoneBodies[0].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[2].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[4].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[6].bodyType = RigidbodyType2D.Dynamic;
            }
            else if (_position == 1)
            {
                _stoneBodies[1].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[3].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[5].bodyType = RigidbodyType2D.Dynamic;
            }
            _position++;
            if (_position == 2)
            {
                _position = 0;
                _isHole = false;
            }
            _holeTimer = timeHole;
        }
    }

    void MiddleAfterOther()
    {
        _middleTimer -= Time.deltaTime;
        if (_middleTimer < 0)
        {
            if (_position == 0)
            {
                _stoneBodies[0].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[1].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[5].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[6].bodyType = RigidbodyType2D.Dynamic;
            }
            else if (_position == 1)
            {
                _stoneBodies[2].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[3].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[4].bodyType = RigidbodyType2D.Dynamic;
            }
            _position++;
            if (_position == 2)
            {
                _position = 0;
                _isMiddle = false;
            }
            _middleTimer = timeMiddle;
        }
    }

    void StairsRain()
    {
        _stairsTimer -= Time.deltaTime;
        if (_stairsTimer < 0)
        {
            if (_position == 0)
            {
                _stoneBodies[0].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[1].bodyType = RigidbodyType2D.Dynamic;
            }
            else if (_position == 1)
            {
                _stoneBodies[2].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[3].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[4].bodyType = RigidbodyType2D.Dynamic;
            }
            else if (_position == 2)
            {
                _stoneBodies[5].bodyType = RigidbodyType2D.Dynamic;
                _stoneBodies[6].bodyType = RigidbodyType2D.Dynamic;
            }
            _position++;
            if (_position == 3)
            {
                _position = 0;
                _isStairs = false;
            }
            _stairsTimer = timeStairs;
        }
    }

    void ToCentreRain()
    {
        _centreTimer -= Time.deltaTime;
        if (_centreTimer < 0)
        {
            _stoneBodies[_position].bodyType = RigidbodyType2D.Dynamic;
            if (_position == 0) _position = 6;
            else if (_position == 6) _position = 1;
            else if (_position == 1) _position = 5;
            else if (_position == 5) _position = 2;
            else if (_position == 2) _position = 4;
            else if (_position == 4) _position = 3;
            else if (_position == 3) _position = 10;
            if (_position == 10)
            {
                _position = 0;
                _isCentre = false;
            }
            _centreTimer = timeCentre;
        }
    }
}
