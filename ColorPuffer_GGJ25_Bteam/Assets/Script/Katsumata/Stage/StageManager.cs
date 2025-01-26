using System;
using System.Collections;
using System.Collections.ObjectModel;
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
internal struct GenerationProbability
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
    private static readonly Vector2 _bubbleSpawnHeight = Vector2.down * 6;

    [Tooltip("�w�i�̖A�𐶐����鉜�s")]
    private static readonly Vector2 _backGroundBubbleSpawnDeep = Vector3.forward * 3;

    [SerializeField, Header("�����ΏہA�����m���A�����̔z��̎��")]
    private GenerationProbability[] _generationProbabilitys = null;

    [SerializeField, Header("�w�i�̖A")]
    private Transform _backGroundBubblePrefab = null;

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

    [SerializeField, Header("�w�i��Animator�z��")]
    private Animator[] _backGroundAnimators = null;

    [Tooltip("���������Q���̔z��")]
    private List<Transform> _obstaclePrefabs = null;

    [Tooltip("��������A�C�e���̔z��")]
    private List<Transform> _itemPrefabs = null;

    [Tooltip("��������A�̔z��")]
    private List<Transform> _bubblePrefabs = null;

    [Tooltip("��������X�e�[�W�C�x���g�̔z��")]
    private List<IStageEvent> _stageEvents = null;

    [Tooltip("�O�񐶐������ʒu")]
    private Vector2 _lastSpawnPoint = Vector2.zero;

    [Tooltip("��������Entity�̃��X�g")]
    private List<Transform> _entitys = null;

    [Tooltip("�����ʒu���㏑������")]
    private Vector2[] _overrideSpawnPoints = null;

    [Tooltip("���������Q�����㏑������")]
    private Transform[] _overrideObstaclePrefabs = null;

    [Tooltip("��Q���𐶐�����Ԋu���㏑������")]
    private float _overrideObstacleSpawnSpan = 0.0f;

    /// <summary>
    /// �ǂݎ���p�̐�������Entity�̃��X�g
    /// </summary>
    public ReadOnlyCollection<Transform> Entitys { get => new(_entitys); }

    private void OnEnable()
    {
        // ������
        _obstaclePrefabs = new();
        _itemPrefabs = new();
        _bubblePrefabs = new();
        _entitys = new();
        _stageEvents = new();

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
        // �A�C�e���������ĂȂ�
        //StartCoroutine(RandomPrefabSpawner(_itemPrefabs.ToArray(), _itemSpawnSpan));

        // �A�𐶐�
        StartCoroutine(RandomBubbleSpawner(_bubblePrefabs.ToArray(), _bubbleSpawnSpan));

        // ��������X�e�[�W�C�x���g�̔z����쐬
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
    /// �����ʒu�̏���������
    /// </summary>
    private SpawnPointsInfo InitializationSpawnPointsInfo(SpawnPointsInfo spawnPointsInfo)
    {
        // ������
        spawnPointsInfo.spawnPoints = new();

        // �����ʒu���v�Z���ă��X�g��
        spawnPointsInfo.spawnPoints.Add(_spawnPointsInfo.leftPoint);
        spawnPointsInfo.spawnPoints.Add(_spawnPointsInfo.rightPoint);

        // ���E�̐����ʒu�����p���ĊԂ̐����ʒu���v�Z
        var centerPoint = (spawnPointsInfo.leftPoint + _spawnPointsInfo.rightPoint).Halve();
        var leftSpacerPoint = (_spawnPointsInfo.leftPoint + centerPoint).Halve();
        var rightSpacerPoint = (_spawnPointsInfo.rightPoint + centerPoint).Halve();

        // ���X�g�ɒǉ�
        spawnPointsInfo.spawnPoints.Add(centerPoint);
        spawnPointsInfo.spawnPoints.Add(leftSpacerPoint);
        spawnPointsInfo.spawnPoints.Add(rightSpacerPoint);

        // �X�ɍו���
        var spacerLeftSpacerPoint = (_spawnPointsInfo.leftPoint + leftSpacerPoint).Halve();
        var spacerRightSpacerPoint = (_spawnPointsInfo.rightPoint + rightSpacerPoint).Halve();
        var centerSpacerLeftSpacerPoint = (centerPoint + leftSpacerPoint).Halve();
        var centerSpacerRightSpacerPoint = (centerPoint + rightSpacerPoint).Halve();

        // ���X�g�ɒǉ�
        spawnPointsInfo.spawnPoints.Add(spacerLeftSpacerPoint);
        spawnPointsInfo.spawnPoints.Add(spacerRightSpacerPoint);
        spawnPointsInfo.spawnPoints.Add(centerSpacerLeftSpacerPoint);
        spawnPointsInfo.spawnPoints.Add(centerSpacerRightSpacerPoint);

        return spawnPointsInfo;
    }

    /// <summary>
    /// ��Q���̃X�|�i�[
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

            // �����ʒu�̏㏑����null��������ʏ�̐����ʒu���g�p
            var spawnPoints = (_overrideSpawnPoints == null) ? _spawnPointsInfo.spawnPoints.ToArray() : _overrideSpawnPoints;

            // ���������Q���̏㏑����null��������ʏ�̐��������Q�����g�p
            var obstaclePrefabs = (_overrideObstaclePrefabs == null) ? _obstaclePrefabs.ToArray() : _overrideObstaclePrefabs;

            var entity = SpawnRandomPrefab(obstaclePrefabs, spawnPoints);

            // �܂��܂܂�Ă��Ȃ���Βǉ�
            AddEntitys(entity);
        }
    }

    /// <summary>
    /// �A�̃X�|�i�[
    /// </summary>
    /// <param name="prefabs"></param>
    /// <param name="span"></param>
    /// <returns></returns>
    private IEnumerator RandomBubbleSpawner(Transform[] prefabs, float span)
    {
        while (true)
        {
            yield return new WaitForSeconds(span);

            // �����_���ȖA�������_���Ȉʒu�ɐ���
            var entity = SpawnRandomBubble(prefabs);

            // �܂��܂܂�Ă��Ȃ���Βǉ�
            AddEntitys(entity);

            // �w�i�̖A��w�i�̃����_���Ȉʒu�ɐ���
            SpawnRandomBackGroundBubble();
        }
    }

    /// <summary>
    /// ��������Entity�������ɒǉ�
    /// </summary>
    /// <param name="entity">��������Entity</param>
    private void AddEntitys(Transform entity)
    {
        // ���Ɋ܂܂�Ă����A�܂���null������
        if (_entitys.Contains(entity) || entity == null) { return; }

        // ���X�g�ɒǉ�
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
    /// �����_���Ȑ����ʒu�Ƀ����_����Prefab�𐶐�
    /// </summary>
    private Transform SpawnRandomPrefab(Transform[] prefabs, Vector2[] spawnPoints)
    {
        if (prefabs == null || prefabs.Length <= 0) { return null; }

        // �����_���Ȑ����ʒu�A�����_����Prefab�𒊑I
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        while (_lastSpawnPoint == spawnPoint)
        {
            // �����_���Ȑ����ʒu�A�����_����Prefab�𒊑I
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        }

        // �Ō�̐����ʒu���X�V
        _lastSpawnPoint = spawnPoint;

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

    /// <summary>
    /// �����_���Ȑ����ʒu�ɔw�i�̖A�𐶐�
    /// </summary>
    private Transform SpawnRandomBackGroundBubble()
    {
        if (_backGroundBubblePrefab == null) { return null; }

        // 0�`1�̊Ԃ̃����_���Ȓl�𐶐�
        float t = Random.value;

        // �����_���Ȑ����ʒu�𒊑I
        var spawnPoint = Vector2.Lerp(_bubbleSpawnPointsInfo.leftPoint, _bubbleSpawnPointsInfo.rightPoint, t);

        // Prefab�𒊑I�����ʒu�ɐ�������
        return Instantiate(_backGroundBubblePrefab, spawnPoint + _bubbleSpawnHeight + _backGroundBubbleSpawnDeep, Quaternion.identity);
    }

    /// <summary>
    /// �����ʒu���㏑��
    /// </summary>
    /// <param name="overrideSpawnPoints"></param>
    public void OverrideSpawnPoints(Vector2[] overrideSpawnPoints) => _overrideSpawnPoints = overrideSpawnPoints;

    /// <summary>
    /// �����ʒu�̏㏑�����I��
    /// </summary>
    public void ResetOverrideSpawnPoints() => _overrideSpawnPoints = null;

    /// <summary>
    /// ���������Q�����㏑��
    /// </summary>
    /// <param name="overrideObstaclePrefabs"></param>
    public void OverrideObstaclePrefabs(Transform[] overrideObstaclePrefabs) => _overrideObstaclePrefabs = overrideObstaclePrefabs;

    /// <summary>
    /// ���������Q���̏㏑�����I��
    /// </summary>
    public void ResetObstaclePrefabs() => _overrideObstaclePrefabs = null;

    /// <summary>
    /// ��Q���𐶐�����Ԋu���㏑��
    /// </summary>
    /// <param name="overrideObstacleSpawnSpan"></param>
    public void OverrideObstacleSpawnSpan(float overrideObstacleSpawnSpan) => _overrideObstacleSpawnSpan = overrideObstacleSpawnSpan;

    /// <summary>
    /// ��Q���𐶐�����Ԋu���㏑�����I��
    /// </summary>
    public void ResetOverrideObstacleSpawnSpan() => _overrideObstacleSpawnSpan = 0.0f;
}
