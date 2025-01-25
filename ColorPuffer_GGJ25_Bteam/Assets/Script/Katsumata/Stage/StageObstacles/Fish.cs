using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
#endif

/// <summary>
/// 小魚
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ObstaclesMovementRange))]
public class Fish : MonoBehaviour, IObstacle
{
    [SerializeField, Min(0.0f), Header("移動速度")]
    private float _speed = 0.0f;

    private Rigidbody2D _myRigidbody = null;

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myRigidbody);
    }

    /// <summary>
    /// PLに当たったらPLから逃げる
    /// </summary>
    public void HitObstacle(/* Player player */Transform player)
    {
        // 移動量を初期化
        _myRigidbody.velocity = Vector2.zero;

        // PLから逃げる
        var dir = transform.position - player.transform.position;
        _myRigidbody.AddForce(dir * _speed, ForceMode2D.Impulse);

        // 逃げている間の処理
        StartCoroutine(Escape());
    }

    /// <summary>
    /// 逃げている間の処理
    /// </summary>
    /// <returns>null</returns>
    private IEnumerator Escape()
    {
        var tfm = transform;

        // 目標角度、速度、方向を定義
        var targetAngle = 360.0f;
        var speed = 1.0f;
        var dir = Vector3.forward;

        // 回転実行
        for (int i = 0; i < targetAngle; i++)
        {
            yield return null;

            transform.Rotate(dir * speed);
        }

        // 角度を実行
        tfm.rotation = Quaternion.identity;
    }
}
