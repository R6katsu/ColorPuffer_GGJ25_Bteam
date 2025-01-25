using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
#endif

/// <summary>
/// ポイント対象の魚
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ObstaclesMovementRange))]
[RequireComponent(typeof(ScrollObstacle))]
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

    [SerializeField, Header("効果音再生用の情報")]
    private PlaySEInfo _playSEInfo = new PlaySEInfo();

    [Tooltip("自身のSpriteRenderer")]
    private SpriteRenderer _mySpriteRenderer = null;

    [Tooltip("自身のRigidbody2D")]
    private Rigidbody2D _myRigidbody = null;

    [Tooltip("自身のScrollObstacle")]
    private ScrollObstacle _myScrollObstacle = null;

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myRigidbody);
        TryGetComponent(out _mySpriteRenderer);
        TryGetComponent(out _myScrollObstacle);

        // 自身の色の種類に紐づけられた色に変更
        _mySpriteRenderer.color = (_fishColors.ContainsKey(_myColorType)) ? _fishColors[_myColorType] : _mySpriteRenderer.color;
    }

    /// <summary>
    /// PLに当たったら吹き飛ぶ
    /// </summary>
    public bool HitObstacle(Player player)
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
            //スコア加算用
            player.HitPoint(_myColorType);
        }
        else
        {
            AudioPlayManager.Instance.PlaySE2D
            (
                3,
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

        return isSuccess;
    }

}
