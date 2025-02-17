using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// 色の種類
/// </summary>
public enum ColorType : byte
{
    [Tooltip("初期値")] Default,
    [Tooltip("赤")] Red,
    [Tooltip("青")] Blue,
    [Tooltip("紫")] Purple
}
