using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

#if UNITY_EDITOR
#endif

/// <summary>
/// �����̔z��̎��
/// </summary>
public enum SpawnType : byte
{
    Obstacle,
    Item,
    Bubble
}

/// <summary>
/// �����ʒu�̏��
/// </summary>
[Serializable]
public struct SpawnPointsInfo
{
    [Header("���̐����ʒu")]
    public Vector2 leftPoint;

    [Header("�E�̐����ʒu")]
    public Vector2 rightPoint;

    [HideInInspector, Tooltip("�����ʒu�̃��X�g")]
    public List<Vector2> spawnPoints;
}

/// <summary>
/// �����m��
/// </summary>
[Serializable]
public struct GenerationProbability
{
    [Header("��������Prefab")]
    public Transform prefab;

    [Min(0), Header("�����m���i�z��ɗe��鐔�j")]
    public int probability;

    [Tooltip("�����̔z��̎��")]
    public SpawnType mySpawnType;
}

/// <summary>
/// �X�e�[�W�Ǘ�
/// </summary>
public class StageManager : MonoBehaviour
{
    [Tooltip("�A�𐶐����鍂��")]
    private static readonly Vector2 _bubbleSpawnHeight = Vector2.down * 5;

    [SerializeField, Header("�����ΏہA�����m���A�����̔z��̎��")]
    private GenerationProbability[] _generationProbabilitys = null;

    [SerializeField, Min(0.0f), Header("��Q���𐶐�����Ԋu")]
    private float _obstacleSpawnSpan = 0.0f;

    [SerializeField, Min(0.0f), Header("�A�C�e���𐶐�����Ԋu")]
    private float _itemSpawnSpan = 0.0f;

    [SerializeField, Min(0.0f), Header("�A�𐶐�����Ԋu")]
    private float _bubbleSpawnSpan = 0.0f;

    [SerializeField, Header("�����ʒu�̏��")]
    private SpawnPointsInfo _spawnPointsInfo = new();

    [SerializeField, Header("�A�̐����ʒu�̏��")]
    private SpawnPointsInfo _bubbleSpawnPointsInfo = new();

    [Tooltip("���������Q���̔z��")]
    private List<Transform> _obstaclePrefabs = null;

    [Tooltip("��������A�C�e���̔z��")]
    private List<Transform> _itemPrefabs = null;

    [Tooltip("��������A�̔z��")]
    private List<Transform> _bubblePrefabs = null;

    private void OnEnable()
    {
        // ������
        _obstaclePrefabs = new();
        _itemPrefabs = new();
        _bubblePrefabs = new();

        // GenerationProbability���g���Đ����Ώۂ̔z����쐬����
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

        // �����ʒu�̏���������
        _spawnPointsInfo = InitializationSpawnPointsInfo(_spawnPointsInfo);
        _bubbleSpawnPointsInfo = InitializationSpawnPointsInfo(_bubbleSpawnPointsInfo);

        // ��Q���A�A�C�e���𐶐�
        StartCoroutine(RandomPrefabSpawner(_obstaclePrefabs.ToArray(), _obstacleSpawnSpan));
        StartCoroutine(RandomPrefabSpawner(_itemPrefabs.ToArray(), _itemSpawnSpan));

        // �A�𐶐�
        StartCoroutine(RandomBubbleSpawner(_bubblePrefabs.ToArray(), _bubbleSpawnSpan));
    }

    /// <summary>
    /// �����ʒu�̏���������
    /// </summary>
    private SpawnPointsInfo InitializationSpawnPointsInfo(SpawnPointsInfo spawnPointsInfo)
    {
        // ������
        spawnPointsInfo.spawnPoints = new();

        // �����ʒu���v�Z���ă��X�g��
        spawnPointsInfo.spawnPoints.Add(_spawnPointsInfo.leftPoint);
        spawnPointsInfo.spawnPoints.Add(_spawnPointsInfo.rightPoint);

        // ���E�̐����ʒu�����p���ĊԂ̐����ʒu���v�Z�A���X�g��
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
    /// �����_���Ȑ����ʒu�Ƀ����_����Prefab�𐶐�
    /// </summary>
    private Transform SpawnRandomPrefab(Transform[] prefabs)
    {
        if (prefabs == null || prefabs.Length <= 0) { return null; }

        // �����_���Ȑ����ʒu�A�����_����Prefab�𒊑I
        var spawnPoint = _spawnPointsInfo.spawnPoints[Random.Range(0, _spawnPointsInfo.spawnPoints.Count)];
        var prefab = prefabs[Random.Range(0, prefabs.Length)];

        // ���I����Prefab�𒊑I�����ʒu�ɐ�������
        return Instantiate(prefab, spawnPoint, Quaternion.identity);
    }

    /// <summary>
    /// �����_���Ȑ����ʒu�ɖA�𐶐�
    /// </summary>
    private Transform SpawnRandomBubble(Transform[] prefabs)
    {
        if (prefabs == null || prefabs.Length <= 0) { return null; }

        // �����_����Prefab�𒊑I
        var prefab = prefabs[Random.Range(0, prefabs.Length)];

        // 0�`1�̊Ԃ̃����_���Ȓl�𐶐�
        float t = Random.value;

        // �����_���Ȑ����ʒu�𒊑I
        var spawnPoint = Vector2.Lerp(_bubbleSpawnPointsInfo.leftPoint, _bubbleSpawnPointsInfo.rightPoint, t);

        // Prefab�𒊑I�����ʒu�ɐ�������
        return Instantiate(prefab, spawnPoint + _bubbleSpawnHeight, Quaternion.identity);
    }
}
