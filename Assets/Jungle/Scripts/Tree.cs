using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tree : MonoBehaviour
{
    [SerializeField] private TilemapRenderer insideTree;
    private TilemapRenderer _tilemapRenderer;
    

    private void Start()
    {
        _tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _tilemapRenderer.enabled = true;
        insideTree.enabled = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _tilemapRenderer.enabled = false;
        insideTree.enabled = false;
    }
}