using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ə����I
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class LarvaeFish : MonoBehaviour, IObstacle
{
    [SerializeField, Min(0), Header("�~���������̓��_")]
    private int _point = 0;

    [SerializeField, Min(0.0f), Header("������ԑ��x")]
    private float _speed = 0.0f;

    [Tooltip("���g��Rigidbody2D")]
    private Rigidbody2D _myRigidbody = null;

    [SerializeField, Header("���ʉ��Đ��p�̏��")]
    private PlaySEInfo _playSEInfo = new PlaySEInfo();

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
    }

    /// <summary>
    /// PL�ɓ��������琁�����
    /// </summary>
    public (bool, int) HitObstacle(Player player)
    {
        // PL���c���ł��Ȃ���Ώ�������

        var isSuccess = true;
        /*
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
        */

        // �ړ��ʂ�������
        _myRigidbody.velocity = Vector2.zero;

        // ���C������
        _myRigidbody.drag = 0.0f;

        // ��]����
        _myRigidbody.AddTorque(_speed, ForceMode2D.Impulse);

        // PL���瓦����
        var dir = transform.position - player.transform.position;
        _myRigidbody.AddForce(dir * _speed, ForceMode2D.Impulse);

        return (isSuccess, _point);
    }
}
