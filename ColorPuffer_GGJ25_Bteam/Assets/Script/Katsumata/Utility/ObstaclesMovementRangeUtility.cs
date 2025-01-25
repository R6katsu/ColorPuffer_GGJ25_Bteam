using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// 障害物が移動可能な範囲
/// </summary>
public static class ObstaclesMovementRangeUtility
{
    /// <summary>
    /// 障害物が移動可能な範囲
    /// </summary>
    public static Vector3 Range { get; } = new Vector2(40.0f, 20.0f);

    /// <summary>
    /// 障害物が移動可能な範囲の中心
    /// </summary>
    public static Vector3 Center { get; } = Vector2.zero;
}
