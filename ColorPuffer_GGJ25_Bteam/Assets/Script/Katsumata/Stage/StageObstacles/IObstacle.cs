#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#endif

/// <summary>
/// 障害物に接触した際の処理を実装
/// </summary>
public interface IObstacle : IDisposable
{
    /// <summary>
    /// 障害物に当たった
    /// </summary>
    public (bool, int) HitObstacle(Player player);

    /// <summary>
    /// 削除時の処理
    /// </summary>
    public Action DieEvent { get; set; }
}
