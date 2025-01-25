using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// スクロールする方向の定義
/// </summary>
static public class ScrollDirectionUtility
{
    /// <summary>
    /// スクロールする方向
    /// </summary>
    static public Vector2 Direction { get; } = Vector2.left;
}
