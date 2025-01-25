using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


#if UNITY_EDITOR
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
        _myRigidbody.AddForce(_surfacedDirection * _surfacedSpeed, ForceMode2D.Force);
    }

    /// <summary>
    /// PL�ɓ���������PL�̐F��ς��ď�����
    /// </summary>
    public void HitObstacle(Player player)
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

        // ��L������A�f���o�����܂�PL�̎q�I�u�W�F�N�g�ɂȂ�
        gameObject.SetActive(false);
        transform.parent = player.transform;
        transform.localPosition = Vector2.zero;
    }
}
