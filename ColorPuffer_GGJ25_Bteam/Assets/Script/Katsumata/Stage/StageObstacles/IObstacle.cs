#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// ��Q���ɐڐG�����ۂ̏���������
/// </summary>
public interface IObstacle
{
    /// <summary>
    /// ��Q���ɓ�������
    /// </summary>
    public bool HitObstacle(Player player);
}
