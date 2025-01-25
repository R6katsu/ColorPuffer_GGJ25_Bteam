using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 囚われた魚
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ObstaclesMovementRange))]
[RequireComponent(typeof(LinerMoveAnimation2D))]

public class TrappedFish : MonoBehaviour, IObstacle
{
    [Tooltip("助けるのに必要な色")]
    private const ColorType HELPED_COLOR = ColorType.Purple;

    // 上の点とLineRendererで繋げる（釣り糸）
    // 魚は左右に動いている（行き来ループ）
    // 魚に紫状態で接触するとLineRendererが切れて魚が逃げ出す

    // 初期地点からnマス上の位置と自身の位置をLineRendererで繋げる
    // AnimClipを作成して、左右に2,-2行き来する
    // この2という値は変更可とする

    /// <summary>
    /// PLに当たったら釣り針から助けられる
    /// </summary>
    public void HitObstacle(/* Player player */Transform player)
    {
        //if (HELPED_COLOR != /* PLの色 */) { return; }

        
    }
}
