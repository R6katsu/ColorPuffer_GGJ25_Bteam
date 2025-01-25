using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �|�C���g�Ώۂ̋�
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ObstaclesMovementRange))]
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

    [Tooltip("���g��SpriteRenderer")]
    private SpriteRenderer _mySpriteRenderer = null;

    [Tooltip("���g��Rigidbody2D")]
    private Rigidbody2D _myRigidbody = null;

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myRigidbody);
        TryGetComponent(out _mySpriteRenderer);

        // ���g�̐F�̎�ނɕR�Â���ꂽ�F�ɕύX
        _mySpriteRenderer.color = (_fishColors.ContainsKey(_myColorType)) ? _fishColors[_myColorType] : _mySpriteRenderer.color;
    }

    /// <summary>
    /// PL�ɓ��������琁�����
    /// </summary>
    public void HitObstacle(/* Player player */Transform player)
    {
        // �ړ��ʂ�������
        _myRigidbody.velocity = Vector2.zero;

        // ��]����
        _myRigidbody.AddTorque(_speed, ForceMode2D.Impulse);

        // PL���瓦����
        var dir = transform.position - player.transform.position;
        _myRigidbody.AddForce(dir * _speed, ForceMode2D.Impulse);
    }
}
