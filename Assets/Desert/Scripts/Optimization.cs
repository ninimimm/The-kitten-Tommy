using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimization : MonoBehaviour
{
    [SerializeField] private GameObject[] _gameObjects;

    [SerializeField] private Transform cat;
    // Update is called once per frame
    void Update()
    {
        foreach (var gameObject in _gameObjects)
        {
            if (Math.Abs(cat.position.x - gameObject.transform.position.x) < 8)
                gameObject.SetActive(true);
            else gameObject.SetActive(false);
        }
    }
}
