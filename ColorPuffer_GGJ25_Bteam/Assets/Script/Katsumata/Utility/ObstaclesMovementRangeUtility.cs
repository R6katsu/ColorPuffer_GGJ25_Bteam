using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// ��Q�����ړ��\�Ȕ͈�
/// </summary>
public static class ObstaclesMovementRangeUtility
{
    /// <summary>
    /// ��Q�����ړ��\�Ȕ͈�
    /// </summary>
    public static Vector3 Range { get; } = new Vector2(20.0f, 12.0f);

    /// <summary>
    /// ��Q�����ړ��\�Ȕ͈͂̒��S
    /// </summary>
    public static Vector3 Center { get; } = Vector2.zero;
}
