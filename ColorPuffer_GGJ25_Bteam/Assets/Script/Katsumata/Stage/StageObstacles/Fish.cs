using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using TMPro;
using Unity.VisualScripting;
#endif

/// <summary>
/// 小魚
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
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
    }
}
