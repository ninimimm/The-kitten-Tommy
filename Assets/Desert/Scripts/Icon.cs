using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    [SerializeField] private Vector3 movingVector;
    [SerializeField] private float speed;
    [SerializeField] private float moveTime;
    private float _timer = -0.1f;

    void Update()
    {
        if (_timer < 0)
        {
            _timer = moveTime;
            movingVector.y *= -1;
            movingVector.x *= -1;
        }
        if (_timer >= 0)
        {
            transform.position += speed * Time.deltaTime * movingVector;
            _timer -= Time.deltaTime;
        }
    }
}
