using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// 範囲外に出たら削除する
/// </summary>
public class ObstaclesMovementRange : MonoBehaviour
{
    private Vector2 _minMovementRange = Vector2.zero;
    private Vector2 _maxMovementRange = Vector2.zero;

    private void OnEnable()
    {
        // 半分の大きさを求める
        var halfMovementRange = ObstaclesMovementRangeUtility.Range.Halve();

        // 中心位置を考慮した移動可能範囲を求める
        _minMovementRange = ObstaclesMovementRangeUtility.Center + -halfMovementRange;
        _maxMovementRange = ObstaclesMovementRangeUtility.Center + halfMovementRange;
    }

    private void Update()
    {
        // 移動可能範囲外だった
        if (transform.position.x < _minMovementRange.x || transform.position.x > _maxMovementRange.x ||
            transform.transform.position.y < _minMovementRange.y || transform.position.y > _maxMovementRange.y)
        {
            // 削除
            Destroy(gameObject);
        }
    }
}
