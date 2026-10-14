using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraAdaptation : MonoBehaviour
{
    [Header("Screen.width/Screen.height*2*orthographicSize")]
    [Header("游戏的有效内容宽度")]
    public float vaildWidth = 1.5f;
    private void Awake()
    {
        Adaptation();
    }


    private void Adaptation()
    {
        float aspectRatio = Screen.width * 1f / Screen.height;
        float height = vaildWidth / aspectRatio;
        float orthographicSize = height / 2;

        var camera = GetComponent<Camera>();
        camera.orthographicSize = orthographicSize;
    }

#if UNITY_EDITOR

    [Button]
    private void Refresh()
    {
        Adaptation();
    }

#endif
}