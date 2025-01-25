using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


#if UNITY_EDITOR
using System.Collections.ObjectModel;
using System.Collections;
#endif

/// <summary>
/// 泡
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ObstaclesMovementRange))]
public class Bubble : MonoBehaviour, IObstacle
{
    [Tooltip("浮上する方向")]
    private static readonly Vector2 _surfacedDirection = Vector2.up;

    [SerializeField, Min(0.0f), Header("浮上速度")]
    private float _surfacedSpeed = 0.0f;

    [SerializeField, Header("泡の見た目の配列")]
    private Sprite[] _bubbleSprites = null;

    [SerializeField, Header("自身の色の種類")]
    private ColorType _myColorType = ColorType.Default;

    [SerializeField, Header("効果音再生用の情報")]
    private PlaySEInfo _playSEInfo = new PlaySEInfo();

    [Tooltip("自身のRigidbody")]
    private Rigidbody2D _myRigidbody = null;

    [Tooltip("自身のSpriteRenderer")]
    private SpriteRenderer _mySpriteRenderer = null;

    [Tooltip("泡の種類とSpriteの辞書")]
    private Dictionary<ColorType, Sprite> _bubbleColorSprites = new();

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myRigidbody);
        TryGetComponent(out _mySpriteRenderer);

        // 泡の種類とSpriteの辞書を初期化
        _bubbleColorSprites = new()
        {
            { ColorType.Default, _bubbleSprites[(int)ColorType.Default] },
            { ColorType.Red, _bubbleSprites[(int)ColorType.Red] },
            { ColorType.Blue, _bubbleSprites[(int)ColorType.Blue] }
        };

        // 自身の色の種類に紐づけられたSpriteに変更
        _mySpriteRenderer.sprite = (_bubbleColorSprites.ContainsKey(_myColorType)) ? _bubbleColorSprites[_myColorType] : _mySpriteRenderer.sprite;
    }

    private void FixedUpdate()
    {
        _myRigidbody.AddForce(_surfacedDirection * _surfacedSpeed, ForceMode2D.Force);
    }

    /// <summary>
    /// PLに当たったらPLの色を変えて消える
    /// </summary>
    public void HitObstacle(Player player)
    {
        // 泡を取得時の効果音を再生
        AudioPlayManager.Instance.PlaySE2D
        (
            (int)_playSEInfo.mySENumber,
            _playSEInfo.minPitch,
            _playSEInfo.maxPitch,
            _playSEInfo.volume
        );

        // PLの色を変更する
        player.HitObstacle(_myColorType);

        // 非有効化後、吐き出されるまでPLの子オブジェクトになる
        gameObject.SetActive(false);
        transform.parent = player.transform;
        transform.localPosition = Vector2.zero;
    }
}
