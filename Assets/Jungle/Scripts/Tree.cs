using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tree : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] crates;
    [SerializeField] private Canvas[] peoplesCanvas;
    [SerializeField] private SpriteRenderer[] peoples;
    [SerializeField] private TilemapRenderer insideTree;
    [SerializeField] private TilemapRenderer wall;
    [SerializeField] private SpriteRenderer[] shoots;
    [SerializeField] private CatSprite cat;
    private TilemapRenderer _tilemapRenderer;
    

    private void Start()
    {
        _tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CatSprite>() != null)
        {
            _tilemapRenderer.enabled = true;
            insideTree.enabled = true;
            wall.enabled = true;
            foreach (var shoot in shoots)
                shoot.enabled = true;
            foreach (var people in peoples)
                people.enabled = true;
            foreach (var peopleCanvas in peoplesCanvas)
                peopleCanvas.enabled = true;
            foreach (var crate in crates)
                if (!crate.gameObject.GetComponent<Crate>().isUse)
                    crate.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CatSprite>() != null && !cat.isOnWall)
        {
            _tilemapRenderer.enabled = false;
            insideTree.enabled = false;
            wall.enabled = false;
            foreach (var shoot in shoots)
                shoot.enabled = false;
            foreach (var people in peoples)
                people.enabled = false;
            foreach (var peopleCanvas in peoplesCanvas)
                peopleCanvas.enabled = false;
            foreach (var crate in crates)
                if (!crate.gameObject.GetComponent<Crate>().isUse)
                    crate.enabled = false;
        }
    }
}
