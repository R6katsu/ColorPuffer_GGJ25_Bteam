using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ꂽ��
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ObstaclesMovementRange))]
[RequireComponent(typeof(LinerMoveAnimation2D))]

public class TrappedFish : MonoBehaviour, IObstacle
{
    [Tooltip("������̂ɕK�v�ȐF")]
    private const ColorType HELPED_COLOR = ColorType.Purple;

    // ��̓_��LineRenderer�Ōq����i�ނ莅�j
    // ���͍��E�ɓ����Ă���i�s�������[�v�j
    // ���Ɏ���ԂŐڐG�����LineRenderer���؂�ċ��������o��

    // �����n�_����n�}�X��̈ʒu�Ǝ��g�̈ʒu��LineRenderer�Ōq����
    // AnimClip���쐬���āA���E��2,-2�s��������
    // ����2�Ƃ����l�͕ύX�Ƃ���

    /// <summary>
    /// PL�ɓ���������ނ�j���珕������
    /// </summary>
    public void HitObstacle(/* Player player */Transform player)
    {
        //if (HELPED_COLOR != /* PL�̐F */) { return; }

        
    }
}
