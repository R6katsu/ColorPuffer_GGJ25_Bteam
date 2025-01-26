using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
#endif

/// <summary>
/// 障害物をスクロールさせる
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class ScrollObstacle : MonoBehaviour
{
    [SerializeField, Min(0.0f), Header("スクロール速度")]
    private float _scrollSpeed = 0.0f;

    private Rigidbody2D _myRigidbody = null;

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myRigidbody);

        // 重力を無効化
        _myRigidbody.gravityScale = 0.0f;
    }

    private void FixedUpdate()
    {
        if (!ScrollUtility.IsScroll) { return; }

        // スクロールする方向に移動
        _myRigidbody.AddForce(ScrollUtility.Direction * _scrollSpeed, ForceMode2D.Force);
    }
}
