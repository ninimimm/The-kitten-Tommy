using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
class EnemyFollow : MonoBehaviour
{
    GameObject player;

    const float speedMove = 30.0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        float direction = player.transform.position.x - transform.position.x;

        if (Mathf.Abs(direction) < 20)
        {
            Vector3 pos = transform.position;
            pos.x += Mathf.Sign(direction) * speedMove * Time.deltaTime;
            transform.position = pos;



        }
    }
}