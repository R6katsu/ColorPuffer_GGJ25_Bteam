using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
#endif

/// <summary>
/// ��Q�����X�N���[��������
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class ScrollObstacle : MonoBehaviour
{
    [SerializeField, Min(0.0f), Header("�X�N���[�����x")]
    private float _scrollSpeed = 0.0f;

    private Rigidbody2D _myRigidbody = null;

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myRigidbody);
    }

    private void FixedUpdate()
    {
        // �X�N���[����������Ɉړ�
        _myRigidbody.AddForce(ScrollDirectionUtility.ScrollDirection * _scrollSpeed, ForceMode2D.Force);
    }
}
