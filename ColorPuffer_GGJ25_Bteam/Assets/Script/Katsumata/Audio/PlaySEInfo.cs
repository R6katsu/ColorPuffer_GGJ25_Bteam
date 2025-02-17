using System;
using UnityEngine;

/// <summary>
/// PlaySEの引数用の情報
/// </summary>
[Serializable]
public struct PlaySEInfo
{
    [Header("効果音番号")]
    public SENumber mySENumber;

    [Min(0.0f)]
    public float minPitch;

    [Min(0.0f)]
    public float maxPitch;

    [Min(0.0f)]
    public float volume;
}