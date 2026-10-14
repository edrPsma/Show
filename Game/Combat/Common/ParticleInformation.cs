using System.Collections;
using System.Collections.Generic;
using Combat;
using Sirenix.OdinInspector;
using UnityEngine;

public class ParticleInformation : MonoController
{
    [LabelText("是否循环")] public bool _isLoop;
    [LabelText("持续时间"), ShowIf(nameof(ShowDuration))] public float Duration = 3;

    public bool IsLoop => _isLoop;

    ParticleSystem[] _particles;
    float _lastSpeed = 1;
    bool _initDone;

    void Init()
    {
        if (_initDone) return;

        _particles = GetComponentsInChildren<ParticleSystem>();
        _initDone = true;
    }

    public void Play(float speed = 1)
    {
        Init();
        SetSpeed(speed);
        _particles[0].Play(true);
    }

    void SetSpeed(float speed)
    {
        if (speed == _lastSpeed) return;

        for (int i = 0; i < _particles.Length; i++)
        {
            var main = _particles[i].main;
            main.simulationSpeed = speed;
        }
        _lastSpeed = speed;
    }

    bool ShowDuration() => !_isLoop;
}