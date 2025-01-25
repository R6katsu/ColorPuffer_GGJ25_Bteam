using UnityEngine;

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// ���l�n�̊g���֐�
/// </summary>
public static class NumericExtensions
{
    [Tooltip("�����̒l")]
    private const int HALVE_NUMBER = 2;

    /// <summary>
    /// int�̊g���֐��B<br/>
    /// �����̒l��Ԃ�
    /// </summary>
    /// <param name="value">�����Ɋ���l</param>
    /// <returns>�����̒l</returns>
    public static int Halve(this int value)
    {
        return value / HALVE_NUMBER;
    }

    /// <summary>
    /// float�̊g���֐��B<br/>
    /// �����̒l��Ԃ�
    /// </summary>
    /// <param name="value">�����Ɋ���l</param>
    /// <returns>�����̒l</returns>
    public static float Halve(this float value)
    {
        return value / HALVE_NUMBER;
    }

    /// <summary>
    /// Vector2�̊g���֐��B<br/>
    /// �����̒l��Ԃ�
    /// </summary>
    /// <param name="value">�����Ɋ���l</param>
    /// <returns>�����̒l</returns>
    public static Vector2 Halve(this Vector2 value)
    {
        return value / HALVE_NUMBER;
    }

    /// <summary>
    /// Vector3�̊g���֐��B<br/>
    /// �����̒l��Ԃ�
    /// </summary>
    /// <param name="value">�����Ɋ���l</param>
    /// <returns>�����̒l</returns>
    public static Vector3 Halve(this Vector3 value)
    {
        return value / HALVE_NUMBER;
    }
}
