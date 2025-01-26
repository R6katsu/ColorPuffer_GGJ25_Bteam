using System.Collections;
using UnityEngine;

/// <summary>
/// ��ʔ����C�x���g
/// </summary>
public class MassOutbreak : MonoBehaviour, IStageEvent
{
    [Tooltip("�A�j���[�V�����̒����̍ő�l")]
    const float DEFAULT_ANIMATION_LENGTH = 1.0f;

    [SerializeField, Min(0),  Header("�C�x���g����������m��")]
    private int _eventProbability = 0;

    [SerializeField, Min(0), Header("�C�x���g�̒���")]
    private int _eventLength = 0;

    [SerializeField, Min(0.0f), Header("��Q���𐶐�����Ԋu")]
    private float _overrideObstacleSpawnSpan = 0.0f;

    [SerializeField, Header("��ʔ����������Q��")]
    private Transform[] _overrideObstaclePrefabs = null;

    [SerializeField, Header("�C�x���g�̖��̂��������A�j���[�V����")]
    private Animator _eventNameAnimator = null;

    [SerializeField, Header("�V�[���J�ڊJ�n�̍Đ�Trigger")]
    private string _startTriggerName = null;

    [SerializeField, Header("�V�[���J�ڏI���̍Đ�Trigger")]
    private string _endTriggerName = null;

    /// <summary>
    /// �C�x���g�̒���
    /// </summary>
    public int EventLength { get => _eventLength; }

    /// <summary>
    /// �C�x���g����������m��
    /// </summary>
    public int EventProbability { get => _eventProbability; }

    /// <summary>
    /// �X�e�[�W�Ŕ�������C�x���g
    /// </summary>
    public IEnumerator StageEvent(StageManager stageManager)//EndTrigger
    {
        // ��Q���𐶐�����Ԋu���㏑������
        stageManager.OverrideObstacleSpawnSpan(_overrideObstacleSpawnSpan);

        // ���������Q�����㏑������
        stageManager.OverrideObstaclePrefabs(_overrideObstaclePrefabs);

        // �J�n�A�j���[�V�����Đ�
        StartCoroutine(PlayAnimationAndWait(_startTriggerName));

        // �C�x���g�I���܂őҋ@
        yield return new WaitForSeconds(EventLength);

        // �I���A�j���[�V�����Đ�
        StartCoroutine(PlayAnimationAndWait(_endTriggerName));

        // �������o��������
        stageManager.ResetOverrideObstacleSpawnSpan();

        // ���������Q����������
        stageManager.ResetObstaclePrefabs();
    }

    /// <summary>
    /// �A�j���[�V�������Đ����A�I���܂őҋ@����
    /// </summary>
    /// <param name="triggerName">�A�j���[�V�����Đ�����Trigger�̖���</param>
    /// <returns></returns>
    public IEnumerator PlayAnimationAndWait(string triggerName)
    {
        // �g�����W�V�������I��
        _eventNameAnimator.SetTrigger(triggerName);

        // �A�j���[�V�����̑J�ڂ�҂�
        yield return null;

        // �A�j���[�V�������̎擾
        var stateInfo = _eventNameAnimator.GetCurrentAnimatorStateInfo(0);

        // �������������擾�ł���܂őҋ@
        while (stateInfo.length == DEFAULT_ANIMATION_LENGTH)
        {
            yield return null;
            stateInfo = _eventNameAnimator.GetCurrentAnimatorStateInfo(0);
        }

        // �A�j���[�V�����I���܂őҋ@
        var animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);
    }
}
