using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private bool moveRight;
    [SerializeField] private GameObject nutPrefab;
    [SerializeField] private float timeToWait;
    [SerializeField] private float timeToShoot;

    private float timer;

    private GameObject instantiateNut;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeToShoot;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToWait > 0)
            timeToWait -= Time.deltaTime;
        else if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            if (moveRight)
            {
                instantiateNut = Instantiate(nutPrefab, gameObject.transform.position+new Vector3(0.4f,0,0), Quaternion.identity);
                instantiateNut.transform.Rotate(0,0,90);
            }
            else
            {
                instantiateNut = Instantiate(nutPrefab, gameObject.transform.position+new Vector3(-0.4f,0,0), Quaternion.identity);
                instantiateNut.transform.Rotate(0,0,-90);
            }
            instantiateNut.GetComponent<MoveNut>().moveRight = moveRight;
            timer = timeToShoot;
        }
    }
}
