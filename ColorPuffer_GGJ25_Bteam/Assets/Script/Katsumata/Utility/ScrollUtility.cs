using UnityEngine;
using System;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// �X�N���[���̒�`
/// </summary>
static public class ScrollUtility
{
    /// <summary>
    /// �X�N���[���������
    /// </summary>
    static public Vector2 Direction { get; } = Vector2.left;

    /// <summary>
    /// �X�N���[�����邩
    /// </summary>
    static public bool IsScroll { get; private set; } = true;

    /// <summary>
    /// IsScroll�t���O��؂�ւ���
    /// </summary>
    static public void ChangeIsScroll(Type types, bool isScroll)
    {

        Debug.Log(types);
        if (types != typeof(GameManager) && types != typeof(Result)) { return; }

        IsScroll = isScroll;
    }
}
