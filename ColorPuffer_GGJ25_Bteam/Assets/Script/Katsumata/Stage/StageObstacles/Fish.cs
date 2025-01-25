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
/// ����
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
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
    /// PL�ɓ���������PL���瓦����
    /// </summary>
    public void HitObstacle(/* Player player */Transform player)
    {
        // �ړ��ʂ�������
        _myRigidbody.velocity = Vector2.zero;

        // PL���瓦����
        var dir = transform.position - player.transform.position;
        _myRigidbody.AddForce(dir * _speed, ForceMode2D.Impulse);
    }
}
