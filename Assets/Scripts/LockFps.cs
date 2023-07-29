using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockFps : MonoBehaviour
{
    void Awake()
    {
        #if UNITY_STANDALONE_WIN
        Application.targetFrameRate = 240;
        QualitySettings.vSyncCount = 0;
        #elif UNITY_ANDROID     
        Application.targetFrameRate = 60;
        #endif
    }
}
