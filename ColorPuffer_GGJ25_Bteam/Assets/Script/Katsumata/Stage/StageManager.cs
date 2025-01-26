using System;
using System.Collections;
using System.Collections.ObjectModel;
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
internal struct GenerationProbability
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
    private static readonly Vector2 _bubbleSpawnHeight = Vector2.down * 6;

    [Tooltip("背景の泡を生成する奥行")]
    private static readonly Vector2 _backGroundBubbleSpawnDeep = Vector3.forward * 3;

    [SerializeField, Header("生成対象、生成確率、代入先の配列の種類")]
    private GenerationProbability[] _generationProbabilitys = null;

    [SerializeField, Header("背景の泡")]
    private Transform _backGroundBubblePrefab = null;

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

    [SerializeField, Header("背景のAnimator配列")]
    private Animator[] _backGroundAnimators = null;

    [Tooltip("生成する障害物の配列")]
    private List<Transform> _obstaclePrefabs = null;

    [Tooltip("生成するアイテムの配列")]
    private List<Transform> _itemPrefabs = null;

    [Tooltip("生成する泡の配列")]
    private List<Transform> _bubblePrefabs = null;

    [Tooltip("発生するステージイベントの配列")]
    private List<IStageEvent> _stageEvents = null;

    [Tooltip("前回生成した位置")]
    private Vector2 _lastSpawnPoint = Vector2.zero;

    [Tooltip("生成したEntityのリスト")]
    private List<Transform> _entitys = null;

    [Tooltip("生成位置を上書きする")]
    private Vector2[] _overrideSpawnPoints = null;

    [Tooltip("生成する障害物を上書きする")]
    private Transform[] _overrideObstaclePrefabs = null;

    [Tooltip("障害物を生成する間隔を上書きする")]
    private float _overrideObstacleSpawnSpan = 0.0f;

    /// <summary>
    /// 読み取り専用の生成したEntityのリスト
    /// </summary>
    public ReadOnlyCollection<Transform> Entitys { get => new(_entitys); }

    private void OnEnable()
    {
        // 初期化
        _obstaclePrefabs = new();
        _itemPrefabs = new();
        _bubblePrefabs = new();
        _entitys = new();
        _stageEvents = new();

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
        // アイテム実装してない
        //StartCoroutine(RandomPrefabSpawner(_itemPrefabs.ToArray(), _itemSpawnSpan));

        // 泡を生成
        StartCoroutine(RandomBubbleSpawner(_bubblePrefabs.ToArray(), _bubbleSpawnSpan));

        // 発生するステージイベントの配列を作成
        List<IStageEvent> istageEvents = new();
        foreach (var n in GameObject.FindObjectsOfType<Component>())
        {
            var component = n as IStageEvent;
            if (component != null)
            {
                istageEvents.Add(component);
            }
        }

        foreach (var istageEvent in istageEvents)
        {
            for (int i = 0; i < istageEvent.EventProbability; i++)
            {
                _stageEvents.Add(istageEvent);
            }
        }

        var stageEvent = _stageEvents[Random.Range(0, _stageEvents.Count)];
        StartCoroutine(stageEvent.StageEvent(this));
    }

    private void Update()
    {
        foreach (var backGroundAnimator in _backGroundAnimators)
        {
            backGroundAnimator.enabled = ScrollUtility.IsScroll;
        }
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
        var leftSpacerPoint = (_spawnPointsInfo.leftPoint + centerPoint).Halve();
        var rightSpacerPoint = (_spawnPointsInfo.rightPoint + centerPoint).Halve();

        // リストに追加
        spawnPointsInfo.spawnPoints.Add(centerPoint);
        spawnPointsInfo.spawnPoints.Add(leftSpacerPoint);
        spawnPointsInfo.spawnPoints.Add(rightSpacerPoint);

        // 更に細分化
        var spacerLeftSpacerPoint = (_spawnPointsInfo.leftPoint + leftSpacerPoint).Halve();
        var spacerRightSpacerPoint = (_spawnPointsInfo.rightPoint + rightSpacerPoint).Halve();
        var centerSpacerLeftSpacerPoint = (centerPoint + leftSpacerPoint).Halve();
        var centerSpacerRightSpacerPoint = (centerPoint + rightSpacerPoint).Halve();

        // リストに追加
        spawnPointsInfo.spawnPoints.Add(spacerLeftSpacerPoint);
        spawnPointsInfo.spawnPoints.Add(spacerRightSpacerPoint);
        spawnPointsInfo.spawnPoints.Add(centerSpacerLeftSpacerPoint);
        spawnPointsInfo.spawnPoints.Add(centerSpacerRightSpacerPoint);

        return spawnPointsInfo;
    }

    /// <summary>
    /// 障害物のスポナー
    /// </summary>
    /// <param name="prefabs"></param>
    /// <param name="span"></param>
    /// <returns></returns>
    private IEnumerator RandomPrefabSpawner(Transform[] prefabs, float span)
    {
        while (true)
        {
            yield return new WaitForSeconds(span);

            Debug.Log(span);

            // 生成位置の上書きがnullだったら通常の生成位置を使用
            var spawnPoints = (_overrideSpawnPoints == null) ? _spawnPointsInfo.spawnPoints.ToArray() : _overrideSpawnPoints;

            // 生成する障害物の上書きがnullだったら通常の生成する障害物を使用
            var obstaclePrefabs = (_overrideObstaclePrefabs == null) ? _obstaclePrefabs.ToArray() : _overrideObstaclePrefabs;

            var entity = SpawnRandomPrefab(obstaclePrefabs, spawnPoints);

            // まだ含まれていなければ追加
            AddEntitys(entity);
        }
    }

    /// <summary>
    /// 泡のスポナー
    /// </summary>
    /// <param name="prefabs"></param>
    /// <param name="span"></param>
    /// <returns></returns>
    private IEnumerator RandomBubbleSpawner(Transform[] prefabs, float span)
    {
        while (true)
        {
            yield return new WaitForSeconds(span);

            // ランダムな泡をランダムな位置に生成
            var entity = SpawnRandomBubble(prefabs);

            // まだ含まれていなければ追加
            AddEntitys(entity);

            // 背景の泡を背景のランダムな位置に生成
            SpawnRandomBackGroundBubble();
        }
    }

    /// <summary>
    /// 生成したEntityを辞書に追加
    /// </summary>
    /// <param name="entity">生成したEntity</param>
    private void AddEntitys(Transform entity)
    {
        // 既に含まれていた、またはnullだった
        if (_entitys.Contains(entity) || entity == null) { return; }

        // リストに追加
        _entitys.Add(entity);

        if (entity.TryGetComponent(out IObstacle observable))
        {
            observable.DieEvent += () =>
            {
                if (_entitys.Contains(entity))
                {
                    _entitys.Remove(entity);
                }
            };
        }
    }

    /// <summary>
    /// ランダムな生成位置にランダムなPrefabを生成
    /// </summary>
    private Transform SpawnRandomPrefab(Transform[] prefabs, Vector2[] spawnPoints)
    {
        if (prefabs == null || prefabs.Length <= 0) { return null; }

        // ランダムな生成位置、ランダムなPrefabを抽選
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        while (_lastSpawnPoint == spawnPoint)
        {
            // ランダムな生成位置、ランダムなPrefabを抽選
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        }

        // 最後の生成位置を更新
        _lastSpawnPoint = spawnPoint;

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

    /// <summary>
    /// ランダムな生成位置に背景の泡を生成
    /// </summary>
    private Transform SpawnRandomBackGroundBubble()
    {
        if (_backGroundBubblePrefab == null) { return null; }

        // 0～1の間のランダムな値を生成
        float t = Random.value;

        // ランダムな生成位置を抽選
        var spawnPoint = Vector2.Lerp(_bubbleSpawnPointsInfo.leftPoint, _bubbleSpawnPointsInfo.rightPoint, t);

        // Prefabを抽選した位置に生成する
        return Instantiate(_backGroundBubblePrefab, spawnPoint + _bubbleSpawnHeight + _backGroundBubbleSpawnDeep, Quaternion.identity);
    }

    /// <summary>
    /// 生成位置を上書き
    /// </summary>
    /// <param name="overrideSpawnPoints"></param>
    public void OverrideSpawnPoints(Vector2[] overrideSpawnPoints) => _overrideSpawnPoints = overrideSpawnPoints;

    /// <summary>
    /// 生成位置の上書きを終了
    /// </summary>
    public void ResetOverrideSpawnPoints() => _overrideSpawnPoints = null;

    /// <summary>
    /// 生成する障害物を上書き
    /// </summary>
    /// <param name="overrideObstaclePrefabs"></param>
    public void OverrideObstaclePrefabs(Transform[] overrideObstaclePrefabs) => _overrideObstaclePrefabs = overrideObstaclePrefabs;

    /// <summary>
    /// 生成する障害物の上書きを終了
    /// </summary>
    public void ResetObstaclePrefabs() => _overrideObstaclePrefabs = null;

    /// <summary>
    /// 障害物を生成する間隔を上書き
    /// </summary>
    /// <param name="overrideObstacleSpawnSpan"></param>
    public void OverrideObstacleSpawnSpan(float overrideObstacleSpawnSpan) => _overrideObstacleSpawnSpan = overrideObstacleSpawnSpan;

    /// <summary>
    /// 障害物を生成する間隔を上書きを終了
    /// </summary>
    public void ResetOverrideObstacleSpawnSpan() => _overrideObstacleSpawnSpan = 0.0f;
}
