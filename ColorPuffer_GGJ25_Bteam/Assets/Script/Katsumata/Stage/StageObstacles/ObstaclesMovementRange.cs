using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// �͈͊O�ɏo����폜����
/// </summary>
public class ObstaclesMovementRange : MonoBehaviour
{
    private Vector2 _minMovementRange = Vector2.zero;
    private Vector2 _maxMovementRange = Vector2.zero;

    private void OnEnable()
    {
        // �����̑傫�������߂�
        var halfMovementRange = ObstaclesMovementRangeUtility.Range.Halve();

        // ���S�ʒu���l�������ړ��\�͈͂����߂�
        _minMovementRange = ObstaclesMovementRangeUtility.Center + -halfMovementRange;
        _maxMovementRange = ObstaclesMovementRangeUtility.Center + halfMovementRange;
    }

    private void Update()
    {
        // �ړ��\�͈͊O������
        if (transform.position.x < _minMovementRange.x || transform.position.x > _maxMovementRange.x ||
            transform.transform.position.y < _minMovementRange.y || transform.position.y > _maxMovementRange.y)
        {
            // �폜
            Destroy(gameObject);
        }
    }
}
