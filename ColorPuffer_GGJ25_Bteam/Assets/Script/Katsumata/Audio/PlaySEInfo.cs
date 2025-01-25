using System;
using UnityEngine;

/// <summary>
/// PlaySE‚Ìˆø”—p‚Ìî•ñ
/// </summary>
[Serializable]
public struct PlaySEInfo
{
    [Header("Œø‰Ê‰¹”Ô†")]
    public SENumber mySENumber;

    [Min(0.0f)]
    public float minPitch;

    [Min(0.0f)]
    public float maxPitch;

    [Min(0.0f)]
    public float volume;
}