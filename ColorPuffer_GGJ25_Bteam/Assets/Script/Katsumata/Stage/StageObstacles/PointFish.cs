using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
#endif

/// <summary>
/// ポイント対象の魚
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ObstaclesMovementRange))]
[RequireComponent(typeof(ScrollObstacle))]
public class PointFish : MonoBehaviour, IObstacle
{
    private static readonly Dictionary<ColorType, Color> _fishColors = new()
    {
        { ColorType.Red, Color.red },
        { ColorType.Blue, Color.blue }
    };

    [SerializeField, Min(0), Header("救助成功時の得点")]
    private int _point = 0;

    [SerializeField, Min(0.0f), Header("吹き飛ぶ速度")]
    private float _speed = 0.0f;

    [SerializeField, Header("自身の色の種類")]
    private ColorType _myColorType = ColorType.Default;

    [SerializeField, Header("効果音再生用の情報")]
    private PlaySEInfo _playSEInfo = new PlaySEInfo();

    [Tooltip("自身のRigidbody2D")]
    private Rigidbody2D _myRigidbody = null;

    [Tooltip("自身のScrollObstacle")]
    private ScrollObstacle _myScrollObstacle = null;

    /// <summary>
    /// 削除時の処理
    /// </summary>
    public Action DieEvent { get; set; }

    private void OnDisable()
    {
        Dispose();
    }

    public void Dispose()
    {
        DieEvent?.Invoke();
    }

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myRigidbody);
        TryGetComponent(out _myScrollObstacle);
    }

    /// <summary>
    /// PLに当たったら吹き飛ぶ
    /// </summary>
    public (bool, int) HitObstacle(Player player)
    {
        // 同じ色なら助けられる
        var isSuccess = _myColorType == player.CurrentColorType;

        // 助けられる場合のみ効果音を鳴らす
        if (isSuccess)
        {
            // 吹き飛ぶ時の効果音を再生
            AudioPlayManager.Instance.PlaySE2D
            (
                (int)_playSEInfo.mySENumber,
                _playSEInfo.minPitch,
                _playSEInfo.maxPitch,
                _playSEInfo.volume
            );
        }

        // 移動量を初期化
        _myRigidbody.velocity = Vector2.zero;

        // 摩擦無効化
        _myRigidbody.drag = 0.0f;

        // スクロール方向への移動を無効化
        _myScrollObstacle.enabled = false;

        // 回転する
        _myRigidbody.AddTorque(_speed, ForceMode2D.Impulse);

        // PLから逃げる
        var dir = transform.position - player.transform.position;
        _myRigidbody.AddForce(dir * _speed, ForceMode2D.Impulse);

        return (isSuccess, _point);
    }

}
