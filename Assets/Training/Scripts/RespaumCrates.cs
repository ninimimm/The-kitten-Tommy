using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RespaumCrates : MonoBehaviour
{
    [SerializeField] private GameObject[] crates;
    private Vector3[] _transforms;

    private void Start()
    {
        for (var i = 0; i < crates.Length; i++)
            _transforms[i] = crates[i].transform.position;
    }

    // Update is called once per frame
    void Respaum()
    {
        for (var i = 0; i < crates.Length; i++)
            crates[i].transform.position = _transforms[i];
    }
}
