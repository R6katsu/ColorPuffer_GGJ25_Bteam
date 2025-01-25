using UnityEngine;

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// 数値系の拡張関数
/// </summary>
public static class NumericExtensions
{
    [Tooltip("半分の値")]
    private const int HALVE_NUMBER = 2;

    /// <summary>
    /// intの拡張関数。<br/>
    /// 半分の値を返す
    /// </summary>
    /// <param name="value">半分に割る値</param>
    /// <returns>半分の値</returns>
    public static int Halve(this int value)
    {
        return value / HALVE_NUMBER;
    }

    /// <summary>
    /// floatの拡張関数。<br/>
    /// 半分の値を返す
    /// </summary>
    /// <param name="value">半分に割る値</param>
    /// <returns>半分の値</returns>
    public static float Halve(this float value)
    {
        return value / HALVE_NUMBER;
    }

    /// <summary>
    /// Vector2の拡張関数。<br/>
    /// 半分の値を返す
    /// </summary>
    /// <param name="value">半分に割る値</param>
    /// <returns>半分の値</returns>
    public static Vector2 Halve(this Vector2 value)
    {
        return value / HALVE_NUMBER;
    }

    /// <summary>
    /// Vector3の拡張関数。<br/>
    /// 半分の値を返す
    /// </summary>
    /// <param name="value">半分に割る値</param>
    /// <returns>半分の値</returns>
    public static Vector3 Halve(this Vector3 value)
    {
        return value / HALVE_NUMBER;
    }
}
