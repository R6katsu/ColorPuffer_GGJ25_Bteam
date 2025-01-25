using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

#if UNITY_EDITOR
#endif

/// <summary>
/// 代入先の配列の種類
/// </summary>
public enum SpawnType : byte
{
    Obstacle,
    Item,
    Bubble
}

/// <summary>
/// 生成位置の情報
/// </summary>
[Serializable]
public struct SpawnPointsInfo
{
    [Header("左の生成位置")]
    public Vector2 leftPoint;

    [Header("右の生成位置")]
    public Vector2 rightPoint;

    [HideInInspector, Tooltip("生成位置のリスト")]
    public List<Vector2> spawnPoints;
}

/// <summary>
/// 生成確率
/// </summary>
[Serializable]
public struct GenerationProbability
{
    [Header("生成するPrefab")]
    public Transform prefab;

    [Min(0), Header("生成確率（配列に容れる数）")]
    public int probability;

    [Tooltip("代入先の配列の種類")]
    public SpawnType mySpawnType;
}

/// <summary>
/// ステージ管理
/// </summary>
public class StageManager : MonoBehaviour
{
    [Tooltip("泡を生成する高さ")]
    private static readonly Vector2 _bubbleSpawnHeight = Vector2.down * 5;

    [SerializeField, Header("生成対象、生成確率、代入先の配列の種類")]
    private GenerationProbability[] _generationProbabilitys = null;

    [SerializeField, Min(0.0f), Header("障害物を生成する間隔")]
    private float _obstacleSpawnSpan = 0.0f;

    [SerializeField, Min(0.0f), Header("アイテムを生成する間隔")]
    private float _itemSpawnSpan = 0.0f;

    [SerializeField, Min(0.0f), Header("泡を生成する間隔")]
    private float _bubbleSpawnSpan = 0.0f;

    [SerializeField, Header("生成位置の情報")]
    private SpawnPointsInfo _spawnPointsInfo = new();

    [SerializeField, Header("泡の生成位置の情報")]
    private SpawnPointsInfo _bubbleSpawnPointsInfo = new();

    [Tooltip("生成する障害物の配列")]
    private List<Transform> _obstaclePrefabs = null;

    [Tooltip("生成するアイテムの配列")]
    private List<Transform> _itemPrefabs = null;

    [Tooltip("生成する泡の配列")]
    private List<Transform> _bubblePrefabs = null;

    private void OnEnable()
    {
        // 初期化
        _obstaclePrefabs = new();
        _itemPrefabs = new();
        _bubblePrefabs = new();

        // GenerationProbabilityを使って生成対象の配列を作成する
        foreach (var generationProbability in _generationProbabilitys)
        {
            switch (generationProbability.mySpawnType)
            {
                case SpawnType.Obstacle:
                    for (int i = 0; i < generationProbability.probability; i++)
                    {
                        _obstaclePrefabs.Add(generationProbability.prefab);
                    }
                    continue;

                case SpawnType.Item:
                    for (int i = 0; i < generationProbability.probability; i++)
                    {
                        _itemPrefabs.Add(generationProbability.prefab);
                    }
                    continue;

                case SpawnType.Bubble:
                    for (int i = 0; i < generationProbability.probability; i++)
                    {
                        _bubblePrefabs.Add(generationProbability.prefab);

                    }
                    continue;
            }
        }

        // 生成位置の情報を初期化
        _spawnPointsInfo = InitializationSpawnPointsInfo(_spawnPointsInfo);
        _bubbleSpawnPointsInfo = InitializationSpawnPointsInfo(_bubbleSpawnPointsInfo);

        // 障害物、アイテムを生成
        StartCoroutine(RandomPrefabSpawner(_obstaclePrefabs.ToArray(), _obstacleSpawnSpan));
        StartCoroutine(RandomPrefabSpawner(_itemPrefabs.ToArray(), _itemSpawnSpan));

        // 泡を生成
        StartCoroutine(RandomBubbleSpawner(_bubblePrefabs.ToArray(), _bubbleSpawnSpan));
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

        // 左右の生成位置を活用して間の生成位置を計算、リスト化
        var centerPoint = (spawnPointsInfo.leftPoint + _spawnPointsInfo.rightPoint).Halve();
        spawnPointsInfo.spawnPoints.Add(centerPoint);
        spawnPointsInfo.spawnPoints.Add((_spawnPointsInfo.leftPoint + centerPoint).Halve());
        spawnPointsInfo.spawnPoints.Add((_spawnPointsInfo.rightPoint + centerPoint).Halve());

        return spawnPointsInfo;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefabs"></param>
    /// <param name="span"></param>
    /// <returns></returns>
    private IEnumerator RandomPrefabSpawner(Transform[] prefabs, float span)
    {
        while (true)
        {
            yield return new WaitForSeconds(span);

            SpawnRandomPrefab(prefabs);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefabs"></param>
    /// <param name="span"></param>
    /// <returns></returns>
    private IEnumerator RandomBubbleSpawner(Transform[] prefabs, float span)
    {
        while (true)
        {
            yield return new WaitForSeconds(span);

            SpawnRandomBubble(prefabs);
        }
    }

    /// <summary>
    /// ランダムな生成位置にランダムなPrefabを生成
    /// </summary>
    private Transform SpawnRandomPrefab(Transform[] prefabs)
    {
        if (prefabs == null || prefabs.Length <= 0) { return null; }

        // ランダムな生成位置、ランダムなPrefabを抽選
        var spawnPoint = _spawnPointsInfo.spawnPoints[Random.Range(0, _spawnPointsInfo.spawnPoints.Count)];
        var prefab = prefabs[Random.Range(0, prefabs.Length)];

        // 抽選したPrefabを抽選した位置に生成する
        return Instantiate(prefab, spawnPoint, Quaternion.identity);
    }

    /// <summary>
    /// ランダムな生成位置に泡を生成
    /// </summary>
    private Transform SpawnRandomBubble(Transform[] prefabs)
    {
        if (prefabs == null || prefabs.Length <= 0) { return null; }

        // ランダムなPrefabを抽選
        var prefab = prefabs[Random.Range(0, prefabs.Length)];

        // 0～1の間のランダムな値を生成
        float t = Random.value;

        // ランダムな生成位置を抽選
        var spawnPoint = Vector2.Lerp(_bubbleSpawnPointsInfo.leftPoint, _bubbleSpawnPointsInfo.rightPoint, t);

        // Prefabを抽選した位置に生成する
        return Instantiate(prefab, spawnPoint + _bubbleSpawnHeight, Quaternion.identity);
    }
}
