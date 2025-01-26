#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#endif

/// <summary>
/// ��Q���ɐڐG�����ۂ̏���������
/// </summary>
public interface IObstacle : IDisposable
{
    /// <summary>
    /// ��Q���ɓ�������
    /// </summary>
    public (bool, int) HitObstacle(Player player);

    /// <summary>
    /// �폜���̏���
    /// </summary>
    public Action DieEvent { get; set; }
}
