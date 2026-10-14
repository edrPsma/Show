using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Combat;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Transform MovePoint => _movePoint;

    [Header("CONFIG")]
    [SerializeField] Transform _movePoint;
    [SerializeField] float _cameraTopOffsetValue = -1.5f;

    [Header("Parallax")]
    [SerializeField] Transform _midBackGround;
    [SerializeField] Transform _midBackGround2;
    [SerializeField] Transform _farBackGround;
    [SerializeField] Transform _camera;
    [SerializeField] float _midBackGroundSpeed = 0.5f;
    [SerializeField] float _midBackGroundSpeed2 = 0.5f;
    [SerializeField] float _farBackGroundSpeed = 1;
    float _lastPos;
    Vector3 _midBackGroundStartPoint;
    Vector3 _midBackGroundStartPoint2;
    Vector3 _farBackGroundStartPoint;

    CinemachineVirtualCamera _cinemachineVirtualCamera;
    Vector3 _cameraInitPos;
    float _initCameraOffsetY;
    Vector3 _movePointInitPos;
    float _softZoneHeight;

    private void Awake()
    {
        _movePointInitPos = _movePoint.transform.position;
        _lastPos = _camera.position.x;
        _midBackGroundStartPoint = _midBackGround.position;
        _midBackGroundStartPoint2 = _midBackGround2.position;
        _farBackGroundStartPoint = _farBackGround.position;
        _camera.gameObject.AddComponent<VirtualCameraAdaptation>();
        _cinemachineVirtualCamera = _camera.GetComponent<CinemachineVirtualCamera>();

        CinemachineFramingTransposer cinemachineFramingTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _initCameraOffsetY = cinemachineFramingTransposer.m_TrackedObjectOffset.y;
        _cameraInitPos = _cinemachineVirtualCamera.transform.position;
        _softZoneHeight = cinemachineFramingTransposer.m_SoftZoneHeight;
    }

    private void Update()
    {
        float amountToMove = _camera.position.x - _lastPos;

        _midBackGround.position += new Vector3(amountToMove * _midBackGroundSpeed, 0, 0);
        _midBackGround2.position += new Vector3(amountToMove * _midBackGroundSpeed2, 0, 0);
        _farBackGround.position += new Vector3(amountToMove * _farBackGroundSpeed, 0, 0);

        _lastPos = _camera.position.x;
    }

    public void ResetBackGround()
    {
        SetPos(1);
    }


    public void SetPos(int step)
    {
        MapConfig mapConfig = GameEntry.Resource.GetGlobalCacheAsset<MapConfig>("MapConfig");
        _cinemachineVirtualCamera.Follow = null;

        Vector3 amountToMove = mapConfig.MoveDistance * (step - 1) * Vector3.right;

        _cinemachineVirtualCamera.transform.position = _cameraInitPos + amountToMove;
        _movePoint.position = _movePointInitPos + amountToMove;

        _midBackGround.position = _midBackGroundStartPoint + amountToMove * _midBackGroundSpeed;
        _midBackGround2.position = _midBackGroundStartPoint2 + amountToMove * _midBackGroundSpeed2;
        _farBackGround.position = _farBackGroundStartPoint + amountToMove * _farBackGroundSpeed;

        _lastPos = _cinemachineVirtualCamera.transform.position.x;
        _cinemachineVirtualCamera.Follow = _movePoint;
    }

    public int GetStep()
    {
        MapConfig mapConfig = GameEntry.Resource.GetGlobalCacheAsset<MapConfig>("MapConfig");

        return (int)((_movePoint.position - _movePointInitPos).x / mapConfig.MoveDistance) + 1;
    }

    public void MoveToFurthest()
    {
        _cinemachineVirtualCamera.Follow = null;

        Vector3 amountToMove = 8 * Vector3.right;

        _cinemachineVirtualCamera.transform.position = _cameraInitPos + amountToMove;
        _movePoint.position = _movePointInitPos + amountToMove;

        _midBackGround.position = _midBackGroundStartPoint + amountToMove * _midBackGroundSpeed;
        _midBackGround2.position = _midBackGroundStartPoint2 + amountToMove * _midBackGroundSpeed2;
        _farBackGround.position = _farBackGroundStartPoint + amountToMove * _farBackGroundSpeed;

        _lastPos = _cinemachineVirtualCamera.transform.position.x;
        _cinemachineVirtualCamera.Follow = _movePoint;
    }

    public void MoveCamera(bool top, bool tween)
    {
        CinemachineFramingTransposer cinemachineFramingTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (tween)
        {
            cinemachineFramingTransposer.m_SoftZoneHeight = _softZoneHeight;
        }
        else
        {
            cinemachineFramingTransposer.m_SoftZoneHeight = 0;
        }

        cinemachineFramingTransposer.m_TrackedObjectOffset.y = top ? _initCameraOffsetY + _cameraTopOffsetValue : _initCameraOffsetY;
    }
}
