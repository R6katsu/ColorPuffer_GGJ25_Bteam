using System.Collections;
using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
#endif

/// <summary>
/// ����
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ObstaclesMovementRange))]
public class Fish : MonoBehaviour, IObstacle
{
    [SerializeField, Min(0.0f), Header("�ړ����x")]
    private float _speed = 0.0f;

    private Rigidbody2D _myRigidbody = null;

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myRigidbody);
    }

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

    /// <summary>
    /// PL�ɓ���������PL���瓦����
    /// </summary>
    public bool HitObstacle(Player player)
    {
        // �ړ��ʂ�������
        _myRigidbody.velocity = Vector2.zero;

        // PL���瓦����
        var dir = transform.position - player.transform.position;
        _myRigidbody.AddForce(dir * _speed, ForceMode2D.Impulse);

        // �����Ă���Ԃ̏���
        StartCoroutine(Escape());

        // ��΂�false��Ԃ�����
        return false;
    }

    /// <summary>
    /// �����Ă���Ԃ̏���
    /// </summary>
    /// <returns>null</returns>
    private IEnumerator Escape()
    {
        var tfm = transform;

        // �ڕW�p�x�A���x�A�������`
        var targetAngle = 360.0f;
        var speed = 1.0f;
        var dir = Vector3.forward;

        // ��]���s
        for (int i = 0; i < targetAngle; i++)
        {
            yield return null;

            transform.Rotate(dir * speed);
        }

        // �p�x���m��
        tfm.rotation = Quaternion.identity;
    }
}
