using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �A�̃g�����W�V����
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TransitionHelper))]
public class BubbleTransition : MonoBehaviour, ITransition
{
    [Tooltip("�A�j���[�V�����̒����̍ő�l")]
    const float DEFAULT_ANIMATION_LENGTH = 1.0f;

    [Tooltip("���g�̃C���X�^���X")]
    private BubbleTransition _instance = null;

    [Tooltip("���g��Animator")]
    private Animator _myAnimator = null;

    [SerializeField, Header("�V�[���J�ڊJ�n�̍Đ�Trigger")]
    private string _transitionStartTriggerName = null;

    [SerializeField, Header("�V�[���J�ڏI���̍Đ�Trigger")]
    private string _transitionEndTriggerName = null;

    /// <summary>
    /// �V�[���J�ڂ�Animator
    /// </summary>
    private Animator TransitionAnimator
    {
        get
        {
            // null������
            if (_myAnimator == null)
            {
                // RequireComponent
                TryGetComponent(out _myAnimator);
            }

            return _myAnimator;
        }
    }

    /// <summary>
    /// �g�����W�V�����̊J�n
    /// </summary>
    public void StartTransition(int sceneNumber)
    {
        if (_instance == null)
        {
            _instance = this;

            // �V�[�����ׂ��ő��݂���
            DontDestroyOnLoad(transform.parent.gameObject); // Canvas
            DontDestroyOnLoad(gameObject);
        }

        StartCoroutine(Transition(sceneNumber));
    }

    /// <summary>
    /// �g�����W�V����
    /// </summary>
    public IEnumerator Transition(int sceneNumber)
    {
        // �g�����W�V�������J�n
        yield return StartCoroutine(PlayAnimationAndWait(_transitionStartTriggerName));

        // �V�[���J��
        yield return SceneManager.LoadSceneAsync(sceneNumber);

        // �g�����W�V�������I��
        yield return StartCoroutine(PlayAnimationAndWait(_transitionEndTriggerName));
    }

    /// <summary>
    /// �A�j���[�V�������Đ����A�I���܂őҋ@����
    /// </summary>
    /// <param name="triggerName">�A�j���[�V�����Đ�����Trigger�̖���</param>
    /// <returns></returns>
    public IEnumerator PlayAnimationAndWait(string triggerName)
    {
        // �g�����W�V�������I��
        TransitionAnimator.SetTrigger(triggerName);

        // �A�j���[�V�����̑J�ڂ�҂�
        yield return null;

        // �A�j���[�V�������̎擾
        var stateInfo = TransitionAnimator.GetCurrentAnimatorStateInfo(0);

        // �������������擾�ł���܂őҋ@
        while (stateInfo.length == DEFAULT_ANIMATION_LENGTH)
        {
            yield return null;
            stateInfo = TransitionAnimator.GetCurrentAnimatorStateInfo(0);
        }

        // �A�j���[�V�����I���܂őҋ@
        var animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);
    }
}
