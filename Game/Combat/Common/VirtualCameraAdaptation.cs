using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Cinemachine;

public class VirtualCameraAdaptation : MonoBehaviour
{
    // [Header("Screen.width/Screen.height*2*orthographicSize")]
    // [Header("游戏的有效内容宽度")]
    // public float vaildWidth = 1.5f;
    private void Awake()
    {
        Adaptation();
    }


    private void Adaptation()
    {
        var virtualCamera = GetComponent<CinemachineVirtualCamera>();
        float vaildWidth = virtualCamera.m_Lens.OrthographicSize;

        float aspectRatio = Screen.width * 1f / Screen.height;
        float height = vaildWidth / aspectRatio;
        float orthographicSize = height / 2;

        virtualCamera.m_Lens.OrthographicSize = orthographicSize;

        float offset = vaildWidth * 2 - height;
        float yOffset = Vector3.Dot(transform.up, Vector3.down * offset / 2);
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y += yOffset;
    }
}