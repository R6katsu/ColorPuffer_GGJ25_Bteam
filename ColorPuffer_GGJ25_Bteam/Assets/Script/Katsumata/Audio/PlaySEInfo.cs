using System;
using UnityEngine;

/// <summary>
/// PlaySE�̈����p�̏��
/// </summary>
[Serializable]
public struct PlaySEInfo
{
    [Header("���ʉ��ԍ�")]
    public SENumber mySENumber;

    [Min(0.0f)]
    public float minPitch;

    [Min(0.0f)]
    public float maxPitch;

    [Min(0.0f)]
    public float volume;
}