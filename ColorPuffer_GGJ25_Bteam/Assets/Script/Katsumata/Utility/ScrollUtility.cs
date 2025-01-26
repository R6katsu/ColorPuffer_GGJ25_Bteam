using UnityEngine;
using System;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// スクロールの定義
/// </summary>
static public class ScrollUtility
{
    /// <summary>
    /// スクロールする方向
    /// </summary>
    static public Vector2 Direction { get; } = Vector2.left;

    /// <summary>
    /// スクロールするか
    /// </summary>
    static public bool IsScroll { get; private set; } = true;

    /// <summary>
    /// IsScrollフラグを切り替える
    /// </summary>
    static public void ChangeIsScroll(Type types, bool isScroll)
    {
        //if (types != typeof(GameManager)) { return; }

        IsScroll = isScroll;
    }
}
