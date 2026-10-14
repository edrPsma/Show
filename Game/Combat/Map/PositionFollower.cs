using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollower : MonoBehaviour
{
    [SerializeField] Transform _trans;

    public void SetTarget(Transform transform)
    {
        _trans = transform;
    }

    void LateUpdate()
    {
        if (_trans == null) return;

        transform.position = _trans.position;
    }
}
