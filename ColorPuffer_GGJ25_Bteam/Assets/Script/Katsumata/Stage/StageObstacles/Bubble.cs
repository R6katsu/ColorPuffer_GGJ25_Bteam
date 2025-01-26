using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.Collections.ObjectModel;
using System.Collections;
#endif

/// <summary>
/// �A
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ObstaclesMovementRange))]
public class Bubble : MonoBehaviour, IObstacle
{
    [Tooltip("���シ�����")]
    private static readonly Vector2 _surfacedDirection = Vector2.up;

    [SerializeField, Min(0), Header("�~���������̓��_")]
    private int _point = 0;

    [SerializeField, Min(0.0f), Header("���㑬�x")]
    private float _surfacedSpeed = 0.0f;

    [SerializeField, Header("�A�̌����ڂ̔z��")]
    private Sprite[] _bubbleSprites = null;

    [SerializeField, Header("���g�̐F�̎��")]
    private ColorType _myColorType = ColorType.Default;

    [SerializeField, Header("���ʉ��Đ��p�̏��")]
    private PlaySEInfo _playSEInfo = new PlaySEInfo();

    [Tooltip("���g��Rigidbody")]
    private Rigidbody2D _myRigidbody = null;

    [Tooltip("���g��SpriteRenderer")]
    private SpriteRenderer _mySpriteRenderer = null;

    [Tooltip("�A�̎�ނ�Sprite�̎���")]
    private Dictionary<ColorType, Sprite> _bubbleColorSprites = new();

    /// <summary>
    /// �폜���̏���
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
        TryGetComponent(out _mySpriteRenderer);

        // �A�̎�ނ�Sprite�̎�����������
        _bubbleColorSprites = new()
        {
            { ColorType.Default, _bubbleSprites[(int)ColorType.Default] },
            { ColorType.Red, _bubbleSprites[(int)ColorType.Red] },
            { ColorType.Blue, _bubbleSprites[(int)ColorType.Blue] }
        };

        // ���g�̐F�̎�ނɕR�Â���ꂽSprite�ɕύX
        _mySpriteRenderer.sprite = (_bubbleColorSprites.ContainsKey(_myColorType)) ? _bubbleColorSprites[_myColorType] : _mySpriteRenderer.sprite;
    }

    private void FixedUpdate()
    {
        if (!ScrollUtility.IsScroll) { return; }

        _myRigidbody.AddForce(_surfacedDirection * _surfacedSpeed, ForceMode2D.Force);
    }

    /// <summary>
    /// PL�ɓ���������PL�̐F��ς��ď�����
    /// </summary>
    public (bool, int) HitObstacle(Player player)
    {
        // �A���擾���̌��ʉ����Đ�
        AudioPlayManager.Instance.PlaySE2D
        (
            (int)_playSEInfo.mySENumber,
            _playSEInfo.minPitch,
            _playSEInfo.maxPitch,
            _playSEInfo.volume
        );

        // PL�̐F��ύX����
        player.HitObstacle(_myColorType);

        // ������
        Destroy(gameObject);

        // ��΂�true��Ԃ��A
        return (true, _point);
    }
}
