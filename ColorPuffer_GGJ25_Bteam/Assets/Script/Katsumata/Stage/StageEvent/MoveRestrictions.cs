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
/// ���ڋߒ��ӁI<br/>
/// ��ʉ��������n�ʂƂȂ�A�ړ��͈͂���ʏ㔼���ɐ��������
/// </summary>
public class MoveRestrictions : MonoBehaviour, IStageEvent
{
    [Tooltip("�グ�鍂��")]
    private static readonly Vector2 _loomingHeight = Vector2.up * 5;

    [SerializeField, Header("����オ���Ă���n��")]
    private Transform[] _grounds = null;

    [SerializeField, Min(0.0f), Header("���x")]
    private float _speed = 0.0f;

    [SerializeField, Header("�����ʒu�̏��")]
    private SpawnPointsInfo _spawnPointsInfo = new();

    /// <summary>
    /// �C�x���g�̒���
    /// </summary>
    public int EventLength { get; }

    /// <summary>
    /// �C�x���g����������m��
    /// </summary>
    public int EventProbability { get; }

    /// <summary>
    /// �X�e�[�W�Ŕ�������C�x���g
    /// </summary>
    public IEnumerator StageEvent(StageManager stageManager)
    {
        yield return null;

        // �n�ʂ����X�ɏグ��
        StartCoroutine(LoomingGround(stageManager));

        // �����ʒu���v�Z���A�㏑������
        _spawnPointsInfo = InitializationSpawnPointsInfo(_spawnPointsInfo);
        stageManager.OverrideSpawnPoints(_spawnPointsInfo.spawnPoints.ToArray());
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

        // ���X�g�ɒǉ�
        spawnPointsInfo.spawnPoints.Add(centerPoint);

        foreach (var item in spawnPointsInfo.spawnPoints)
        {
            Debug.Log(item);
        }

        return spawnPointsInfo;
    }

    /// <summary>
    /// �n�ʂ����X�ɏグ��
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoomingGround(StageManager stageMgr)
    {
        yield return null;

        var length = _grounds.Length + stageMgr.Entitys.Count;
        List<Transform> targetTransforms = new();

        // ���̈ʒu��ێ�
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

        // �ړ��ʂ��`
        var movementAmount = 1.0f / _speed;

        // �n�ʂ����X�ɏグ�鏈��
        for (int i = 0; i < _loomingHeight.y * _speed; i++)
        {
            yield return null;

            for (int j = 0; j < length; j++)
            {
                if (targetTransforms[j] == null) { continue; }

                targetTransforms[j].Translate(Vector2.up * movementAmount);
            }
        }

        // �ʒu���m�肳����
        for (int i = 0; i < length; i++)
        {
            if (targetTransforms[i] == null) { continue; }

            targetTransforms[i].position = groundPositions[i] + (Vector3)_loomingHeight;
        }
    }
}
