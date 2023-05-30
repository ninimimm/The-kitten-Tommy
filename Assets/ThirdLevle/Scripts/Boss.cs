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
    [SerializeField] private float minTimeLog;  // Min time for spawning logs
    [SerializeField] private float maxTimeLog;
    [SerializeField] private float minSpawnX;  // Min X coordinate for spawning logs
    [SerializeField] private float maxSpawnX;
    [SerializeField] private float destroyX;

    private bool isFireOn = true;
    public List<Log> logs = new List<Log>();

    private SpriteRenderer[] _spriteRenderers;
    private PolygonCollider2D[] _polygonColliders;

    void Start()
    {
        _spriteRenderers = new SpriteRenderer[fires.Length];
        _polygonColliders = new PolygonCollider2D[fires.Length];
        for (var i = 0; i < fires.Length; i++)
        {
            _polygonColliders[i] = fires[i].GetComponent<PolygonCollider2D>();
            _spriteRenderers[i] = fires[i].GetComponent<SpriteRenderer>();
        }

        StartCoroutine(FireRoutine());
        StartCoroutine(LogRoutine());
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
    
    void ToggleFire(bool fireState)
    {
        for (var i = 0; i < fires.Length; i++)
        {
            _polygonColliders[i].enabled = fireState;
            fires[i].layer = fireState ? 18 : 0;
            _spriteRenderers[i].enabled = fireState;
        }
    }
}
