using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

#if UNITY_EDITOR
#endif

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
/// ステージ管理
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class StageManager : MonoBehaviour
{
    [Tooltip("泡を生成する高さ")]
    private static readonly Vector2 _bubbleSpawnHeight = Vector2.down * 6;

    [SerializeField, Header("生成する障害物の配列")]
    private Transform[] _obstaclePrefabs = null;

    [SerializeField, Header("生成するアイテムの配列")]
    private Transform[] _itemPrefabs = null;

    [SerializeField, Header("生成する泡のPrefab")]
    private Transform _bubblePrefab = null;

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

    [Tooltip("自身のMeshRenderer")]
    private MeshRenderer _myMeshRenderer = null;

    [Tooltip("自身のMaterial")]
    private Material _myMaterial = null;

    [SerializeField] private string texturePropertyName = "_MainTex"; // テクスチャプロパティ名
    [SerializeField] private float speed = 1f; // オフセットの移動速度
    private float offsetX = 0f; // オフセットのX値

    private void OnEnable()
    {
        // 生成位置の情報を初期化
        _spawnPointsInfo = InitializationSpawnPointsInfo(_spawnPointsInfo);
        _bubbleSpawnPointsInfo = InitializationSpawnPointsInfo(_bubbleSpawnPointsInfo);

        // 障害物、アイテム、泡の生成
        StartCoroutine(RandomPrefabSpawner(_obstaclePrefabs, _obstacleSpawnSpan));
        StartCoroutine(RandomPrefabSpawner(_itemPrefabs, _itemSpawnSpan));
        StartCoroutine(RandomPrefabSpawner(_bubblePrefab, _bubbleSpawnSpan));

        // RequireComponent
        TryGetComponent(out _myMeshRenderer);
        _myMaterial = _myMeshRenderer.material;
    }

    private void Update()
    {
        // X方向のオフセットを更新
        offsetX += speed * Time.deltaTime;

        // 0から1の範囲にリセット（ループ処理）
        if (offsetX > 1f)
        {
            offsetX -= 1f;
        }

        // マテリアルにオフセットを設定
        Vector2 offset = new Vector2(offsetX, 0f);
        _myMaterial.SetTextureOffset(texturePropertyName, offset);
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
    /// <param name="prefab"></param>
    /// <param name="span"></param>
    /// <returns></returns>
    private IEnumerator RandomPrefabSpawner(Transform prefab, float span)
    {
        while (true)
        {
            yield return new WaitForSeconds(span);

            SpawnRandomPrefab(prefab);
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
    /// ランダムな生成位置にPrefabを生成
    /// </summary>
    private Transform SpawnRandomPrefab(Transform prefab)
    {
        // 0～1の間のランダムな値を生成
        float t = Random.Range(0f, 1f);

        // ランダムな生成位置を抽選
        var spawnPoint = Vector2.Lerp(_bubbleSpawnPointsInfo.leftPoint, _bubbleSpawnPointsInfo.rightPoint, t);

        // Prefabを抽選した位置に生成する
        return Instantiate(prefab, spawnPoint + _bubbleSpawnHeight, Quaternion.identity);
    }
}
