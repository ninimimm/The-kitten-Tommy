using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private CatSprite cat;
    private RawImage _raw;
    void Start()
    {
        _raw = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _raw.enabled = !_raw.enabled;
            cat.inMiniMap = !cat.inMiniMap;
        }
    }
}
