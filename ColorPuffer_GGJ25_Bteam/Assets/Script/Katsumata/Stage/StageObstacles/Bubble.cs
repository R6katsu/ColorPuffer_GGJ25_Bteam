using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// 泡
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Bubble : MonoBehaviour, IObstacle
{
    [Tooltip("浮上する方向")]
    private static readonly Vector2 _surfacedDirection = Vector2.up;

    [SerializeField, Min(0.0f), Header("浮上速度")]
    private float _surfacedSpeed = 0.0f;

    [Tooltip("自身の色の種類")]
    private ColorType _myColorType = ColorType.Default;

    [Tooltip("自身のRigidbody")]
    private Rigidbody2D _myRigidbody = null;

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myRigidbody);
    }

    private void FixedUpdate()
    {
        _myRigidbody.AddForce(_surfacedDirection * _surfacedSpeed, ForceMode2D.Force);
    }

    /// <summary>
    /// PLに当たったらPLの色を変えて消える
    /// </summary>
    public void HitObstacle(/* Player player */Transform player)
    {
        Debug.Log("PLの色を_myColorTypeに変える");

        // 非有効化後、吐き出されるまでPLの子オブジェクトになる
        gameObject.SetActive(false);
        transform.parent = player;
        transform.localPosition = Vector2.zero;
    }
}
