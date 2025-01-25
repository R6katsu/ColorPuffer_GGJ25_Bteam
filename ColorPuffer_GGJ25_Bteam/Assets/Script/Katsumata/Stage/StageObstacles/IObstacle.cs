using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// 障害物に接触した際の処理を実装
/// </summary>
public interface IObstacle
{
    /// <summary>
    /// 障害物に当たった
    /// </summary>
    public void HitObstacle(/* Player player */Transform player);
}
