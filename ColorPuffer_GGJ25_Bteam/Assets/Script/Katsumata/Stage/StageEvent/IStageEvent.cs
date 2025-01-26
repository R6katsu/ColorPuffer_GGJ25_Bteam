#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endif

/// <summary>
/// �X�e�[�W�C�x���g������
/// </summary>
public interface IStageEvent
{
    #region �C�x���g�̗�
    // ���ڋߒ��ӁI
    // ��ʉ��������n�ʂƂȂ�A�ړ��͈͂���ʏ㔼���ɐ��������

    // �C�̐X�˓��I
    // �C���i�킩�߁j�̌Q���n�сB���E��W�Q����w�̍����킩�߂��ߌi�Ƃ��ė����悤�ɂȂ�

    // �ُ�C�ۂ��I
    // �A�Ǝ��ɂ����̋��݂̂��o������悤�ɂȂ�

    // ��{�ނ肾�I
    // �A�ƒނ��Ă��鋛�݂̂��o������悤�ɂȂ�

    // �����Ə����I
    // ������菬���ȏ������o������悤�ɂȂ�B������菬�������߁A�������Ă��_���[�W���󂯂Ȃ��B
    // �c���ł��Ȃ���Ԃœ�����Ɠ��_�ɂȂ�A�c���ł����Ԃœ�����ƌ��_�ɂȂ�i������ƌ��_�j�B�F�͊֌W�Ȃ�
    #endregion

    /// <summary>
    /// �C�x���g����������m��
    /// </summary>
    public int EventProbability { get; }

    /// <summary>
    /// �C�x���g�̒���
    /// </summary>
    public int EventLength { get; }

    /// <summary>
    /// �X�e�[�W�Ŕ�������C�x���g
    /// </summary>
    public IEnumerator StageEvent(StageManager stageManager);

    // StageManager��n���āA�D�����菈�����ĖႤ�H
    // StageManager���ł������l��p�ӂ��Ă����AStageEvent�̊֐����ł�����
    // �����Ə����ɓo�ꂷ�鋛��Prefab�ȂǁA�C���X�y�N�^����ݒ肪�K�v�Ȃ��̂�����
    // IStageEvent���p������class�̃C���X�y�N�^����ݒ肷��B
    // �܂��A�V�[���ɑ��݂���IStageEvent���p�������S�Ă�class���ŏ��Ɏ擾����
    // ���̎��A�C�x���g����������m�����C���^�[�t�F�[�X�Ŏ������Ă���
    // �C�x���g���������Ă��鎞�A�ʂ̃C�x���g���������Ȃ��悤�ɂ���
}
