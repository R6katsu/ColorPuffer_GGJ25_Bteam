using UnityEngine;

#if UNITY_EDITOR
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
    public void HitObstacle(/* Player player */Transform player);
}
