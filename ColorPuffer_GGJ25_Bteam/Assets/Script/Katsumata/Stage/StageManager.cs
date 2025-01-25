using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

#if UNITY_EDITOR
#endif

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
/// �X�e�[�W�Ǘ�
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class StageManager : MonoBehaviour
{
    [Tooltip("�A�𐶐����鍂��")]
    private static readonly Vector2 _bubbleSpawnHeight = Vector2.down * 6;

    [SerializeField, Header("���������Q���̔z��")]
    private Transform[] _obstaclePrefabs = null;

    [SerializeField, Header("��������A�C�e���̔z��")]
    private Transform[] _itemPrefabs = null;

    [SerializeField, Header("��������A��Prefab")]
    private Transform _bubblePrefab = null;

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

    [Tooltip("���g��MeshRenderer")]
    private MeshRenderer _myMeshRenderer = null;

    [Tooltip("���g��Material")]
    private Material _myMaterial = null;

    [SerializeField] private string texturePropertyName = "_MainTex"; // �e�N�X�`���v���p�e�B��
    [SerializeField] private float speed = 1f; // �I�t�Z�b�g�̈ړ����x
    private float offsetX = 0f; // �I�t�Z�b�g��X�l

    private void OnEnable()
    {
        // �����ʒu�̏���������
        _spawnPointsInfo = InitializationSpawnPointsInfo(_spawnPointsInfo);
        _bubbleSpawnPointsInfo = InitializationSpawnPointsInfo(_bubbleSpawnPointsInfo);

        // ��Q���A�A�C�e���A�A�̐���
        StartCoroutine(RandomPrefabSpawner(_obstaclePrefabs, _obstacleSpawnSpan));
        StartCoroutine(RandomPrefabSpawner(_itemPrefabs, _itemSpawnSpan));
        StartCoroutine(RandomPrefabSpawner(_bubblePrefab, _bubbleSpawnSpan));

        // RequireComponent
        TryGetComponent(out _myMeshRenderer);
        _myMaterial = _myMeshRenderer.material;
    }

    private void Update()
    {
        // X�����̃I�t�Z�b�g���X�V
        offsetX += speed * Time.deltaTime;

        // 0����1�͈̔͂Ƀ��Z�b�g�i���[�v�����j
        if (offsetX > 1f)
        {
            offsetX -= 1f;
        }

        // �}�e���A���ɃI�t�Z�b�g��ݒ�
        Vector2 offset = new Vector2(offsetX, 0f);
        _myMaterial.SetTextureOffset(texturePropertyName, offset);
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
    /// �����_���Ȑ����ʒu��Prefab�𐶐�
    /// </summary>
    private Transform SpawnRandomPrefab(Transform prefab)
    {
        // 0�`1�̊Ԃ̃����_���Ȓl�𐶐�
        float t = Random.Range(0f, 1f);

        // �����_���Ȑ����ʒu�𒊑I
        var spawnPoint = Vector2.Lerp(_bubbleSpawnPointsInfo.leftPoint, _bubbleSpawnPointsInfo.rightPoint, t);

        // Prefab�𒊑I�����ʒu�ɐ�������
        return Instantiate(prefab, spawnPoint + _bubbleSpawnHeight, Quaternion.identity);
    }
}
