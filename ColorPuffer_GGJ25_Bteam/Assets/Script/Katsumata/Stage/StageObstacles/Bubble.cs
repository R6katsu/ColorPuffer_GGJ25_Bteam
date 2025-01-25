using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// �A
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Bubble : MonoBehaviour, IObstacle
{
    [Tooltip("���シ�����")]
    private static readonly Vector2 _surfacedDirection = Vector2.up;

    [SerializeField, Min(0.0f), Header("���㑬�x")]
    private float _surfacedSpeed = 0.0f;

    [Tooltip("���g�̐F�̎��")]
    private ColorType _myColorType = ColorType.Default;

    [Tooltip("���g��Rigidbody")]
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
    /// PL�ɓ���������PL�̐F��ς��ď�����
    /// </summary>
    public void HitObstacle(/* Player player */Transform player)
    {
        Debug.Log("PL�̐F��_myColorType�ɕς���");

        // ��L������A�f���o�����܂�PL�̎q�I�u�W�F�N�g�ɂȂ�
        gameObject.SetActive(false);
        transform.parent = player;
        transform.localPosition = Vector2.zero;
    }
}
