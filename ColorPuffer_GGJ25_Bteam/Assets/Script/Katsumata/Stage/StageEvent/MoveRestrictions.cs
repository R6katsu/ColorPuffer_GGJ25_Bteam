using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 島接近注意！<br/>
/// 画面下半分が地面となり、移動範囲が画面上半分に制限される
/// </summary>
public class MoveRestrictions : MonoBehaviour, IStageEvent
{
    [Tooltip("上げる高さ")]
    private static readonly Vector2 _loomingHeight = Vector2.up * 5;

    [SerializeField, Header("競り上がってくる地面")]
    private Transform[] _grounds = null;

    [SerializeField, Min(0.0f), Header("速度")]
    private float _speed = 0.0f;

    [SerializeField, Header("生成位置の情報")]
    private SpawnPointsInfo _spawnPointsInfo = new();

    /// <summary>
    /// イベントの長さ
    /// </summary>
    public int EventLength { get; }

    /// <summary>
    /// イベントが発生する確率
    /// </summary>
    public int EventProbability { get; }

    /// <summary>
    /// ステージで発生するイベント
    /// </summary>
    public IEnumerator StageEvent(StageManager stageManager)
    {
        yield return null;

        // 地面を徐々に上げる
        StartCoroutine(LoomingGround(stageManager));

        // 生成位置を計算し、上書きする
        _spawnPointsInfo = InitializationSpawnPointsInfo(_spawnPointsInfo);
        stageManager.OverrideSpawnPoints(_spawnPointsInfo.spawnPoints.ToArray());
    }

    /// <summary>
    /// 生成位置の情報を初期化
    /// </summary>
    private SpawnPointsInfo InitializationSpawnPointsInfo(SpawnPointsInfo spawnPointsInfo)
    {
        // 初期化
        spawnPointsInfo.spawnPoints = new();

        // 生成位置を計算してリスト化
        spawnPointsInfo.spawnPoints.Add(_spawnPointsInfo.leftPoint);
        spawnPointsInfo.spawnPoints.Add(_spawnPointsInfo.rightPoint);

        // 左右の生成位置を活用して間の生成位置を計算
        var centerPoint = (spawnPointsInfo.leftPoint + _spawnPointsInfo.rightPoint).Halve();

        // リストに追加
        spawnPointsInfo.spawnPoints.Add(centerPoint);

        foreach (var item in spawnPointsInfo.spawnPoints)
        {
            Debug.Log(item);
        }

        return spawnPointsInfo;
    }

    /// <summary>
    /// 地面を徐々に上げる
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoomingGround(StageManager stageMgr)
    {
        yield return null;

        var length = _grounds.Length + stageMgr.Entitys.Count;
        List<Transform> targetTransforms = new();

        // 元の位置を保持
        List<Vector3> groundPositions = new();

        foreach (var ground in _grounds)
        {
            targetTransforms.Add(ground);
        }
        foreach (var entity in stageMgr.Entitys)
        {
            targetTransforms.Add(entity);
        }
        foreach (var targetTransform in targetTransforms)
        {
            groundPositions.Add(targetTransform.position);
        }

        // 移動量を定義
        var movementAmount = 1.0f / _speed;

        // 地面を徐々に上げる処理
        for (int i = 0; i < _loomingHeight.y * _speed; i++)
        {
            yield return null;

            for (int j = 0; j < length; j++)
            {
                if (targetTransforms[j] == null) { continue; }

                targetTransforms[j].Translate(Vector2.up * movementAmount);
            }
        }

        // 位置を確定させる
        for (int i = 0; i < length; i++)
        {
            if (targetTransforms[i] == null) { continue; }

            targetTransforms[i].position = groundPositions[i] + (Vector3)_loomingHeight;
        }
    }
}
