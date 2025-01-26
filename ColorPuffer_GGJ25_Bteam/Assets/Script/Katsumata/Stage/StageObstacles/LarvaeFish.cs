using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// もっと小魚！
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class LarvaeFish : MonoBehaviour, IObstacle
{
    [SerializeField, Min(0), Header("救助成功時の得点")]
    private int _point = 0;

    [SerializeField, Min(0.0f), Header("吹き飛ぶ速度")]
    private float _speed = 0.0f;

    [Tooltip("自身のRigidbody2D")]
    private Rigidbody2D _myRigidbody = null;

    [SerializeField, Header("効果音再生用の情報")]
    private PlaySEInfo _playSEInfo = new PlaySEInfo();

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
    }

    /// <summary>
    /// PLに当たったら吹き飛ぶ
    /// </summary>
    public (bool, int) HitObstacle(Player player)
    {
        // PLが膨らんでいなければ助けられる

        var isSuccess = true;
        /*
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
        */

        // 移動量を初期化
        _myRigidbody.velocity = Vector2.zero;

        // 摩擦無効化
        _myRigidbody.drag = 0.0f;

        // 回転する
        _myRigidbody.AddTorque(_speed, ForceMode2D.Impulse);

        // PLから逃げる
        var dir = transform.position - player.transform.position;
        _myRigidbody.AddForce(dir * _speed, ForceMode2D.Impulse);

        return (isSuccess, _point);
    }
}
