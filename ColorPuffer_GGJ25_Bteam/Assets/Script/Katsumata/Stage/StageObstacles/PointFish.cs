using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
#endif

/// <summary>
/// �|�C���g�Ώۂ̋�
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

    [SerializeField, Min(0.0f), Header("������ԑ��x")]
    private float _speed = 0.0f;

    [SerializeField, Header("���g�̐F�̎��")]
    private ColorType _myColorType = ColorType.Default;

    [SerializeField, Header("���ʉ��Đ��p�̏��")]
    private PlaySEInfo _playSEInfo = new PlaySEInfo();

    [Tooltip("���g��SpriteRenderer")]
    private SpriteRenderer _mySpriteRenderer = null;

    [Tooltip("���g��Rigidbody2D")]
    private Rigidbody2D _myRigidbody = null;

    [Tooltip("���g��ScrollObstacle")]
    private ScrollObstacle _myScrollObstacle = null;

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myRigidbody);
        TryGetComponent(out _mySpriteRenderer);
        TryGetComponent(out _myScrollObstacle);

        // ���g�̐F�̎�ނɕR�Â���ꂽ�F�ɕύX
        _mySpriteRenderer.color = (_fishColors.ContainsKey(_myColorType)) ? _fishColors[_myColorType] : _mySpriteRenderer.color;
    }

    /// <summary>
    /// PL�ɓ��������琁�����
    /// </summary>
    public bool HitObstacle(Player player)
    {
        // �����F�Ȃ珕������
        var isSuccess = _myColorType == player.CurrentColorType;

        // ��������ꍇ�̂݌��ʉ���炷
        if (isSuccess)
        {
            // ������Ԏ��̌��ʉ����Đ�
            AudioPlayManager.Instance.PlaySE2D
            (
                (int)_playSEInfo.mySENumber,
                _playSEInfo.minPitch,
                _playSEInfo.maxPitch,
                _playSEInfo.volume
            );
            //�X�R�A���Z�p
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

        // �ړ��ʂ�������
        _myRigidbody.velocity = Vector2.zero;

        // ���C������
        _myRigidbody.drag = 0.0f;

        // �X�N���[�������ւ̈ړ��𖳌���
        _myScrollObstacle.enabled = false;

        // ��]����
        _myRigidbody.AddTorque(_speed, ForceMode2D.Impulse);

        // PL���瓦����
        var dir = transform.position - player.transform.position;
        _myRigidbody.AddForce(dir * _speed, ForceMode2D.Impulse);

        return isSuccess;
    }

}
