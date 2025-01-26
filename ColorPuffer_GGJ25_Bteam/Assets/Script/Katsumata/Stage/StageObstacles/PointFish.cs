using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
#endif

/// <summary>
/// �|�C���g�Ώۂ̋�
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ObstaclesMovementRange))]
[RequireComponent(typeof(ScrollObstacle))]
public class PointFish : MonoBehaviour, IObstacle
{
    private static readonly Dictionary<ColorType, Color> _fishColors = new()
    {
        { ColorType.Red, Color.red },
        { ColorType.Blue, Color.blue }
    };

    [SerializeField, Min(0), Header("�~���������̓��_")]
    private int _point = 0;

    [SerializeField, Min(0.0f), Header("������ԑ��x")]
    private float _speed = 0.0f;

    [SerializeField, Header("���g�̐F�̎��")]
    private ColorType _myColorType = ColorType.Default;

    [SerializeField, Header("���ʉ��Đ��p�̏��")]
    private PlaySEInfo _playSEInfo = new PlaySEInfo();

    [Tooltip("���g��Rigidbody2D")]
    private Rigidbody2D _myRigidbody = null;

    [Tooltip("���g��ScrollObstacle")]
    private ScrollObstacle _myScrollObstacle = null;

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
        TryGetComponent(out _myScrollObstacle);
    }

    /// <summary>
    /// PL�ɓ��������琁�����
    /// </summary>
    public (bool, int) HitObstacle(Player player)
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

        return (isSuccess, _point);
    }

}
