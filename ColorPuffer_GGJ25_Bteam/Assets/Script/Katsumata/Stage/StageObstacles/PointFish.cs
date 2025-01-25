using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ポイント対象の魚
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ObstaclesMovementRange))]
public class PointFish : MonoBehaviour, IObstacle
{
    private static readonly Dictionary<ColorType, Color> _fishColors = new()
    {
        { ColorType.Red, Color.red },
        { ColorType.Blue, Color.blue }
    };

    [SerializeField, Min(0.0f), Header("吹き飛ぶ速度")]
    private float _speed = 0.0f;

    [SerializeField, Header("自身の色の種類")]
    private ColorType _myColorType = ColorType.Default;

    [Tooltip("自身のSpriteRenderer")]
    private SpriteRenderer _mySpriteRenderer = null;

    [Tooltip("自身のRigidbody2D")]
    private Rigidbody2D _myRigidbody = null;

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myRigidbody);
        TryGetComponent(out _mySpriteRenderer);

        // 自身の色の種類に紐づけられた色に変更
        _mySpriteRenderer.color = (_fishColors.ContainsKey(_myColorType)) ? _fishColors[_myColorType] : _mySpriteRenderer.color;
    }

    /// <summary>
    /// PLに当たったら吹き飛ぶ
    /// </summary>
    public void HitObstacle(/* Player player */Transform player)
    {
        // 移動量を初期化
        _myRigidbody.velocity = Vector2.zero;

        // 回転する
        _myRigidbody.AddTorque(_speed, ForceMode2D.Impulse);

        // PLから逃げる
        var dir = transform.position - player.transform.position;
        _myRigidbody.AddForce(dir * _speed, ForceMode2D.Impulse);
    }
}
