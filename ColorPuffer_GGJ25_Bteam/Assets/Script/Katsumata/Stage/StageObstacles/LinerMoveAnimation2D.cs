using UnityEngine;
using System;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// �����ړ���AnimationClip���쐬���A�Đ�����i2D�j
/// </summary>
[RequireComponent(typeof(Animation))]
public class LinerMoveAnimation2D : MonoBehaviour
{
    [Tooltip("�쐬�����A�j���[�V�����N���b�v�̖��O")]
    private const string CLIP_NAME = "LinerMoveAnimation2D";

    [Tooltip("localPosition.x")]
    private const string LOCAL_POSITION_X = "localPosition.x";

    [Tooltip("localPosition.y")]
    private const string LOCAL_POSITION_Y = "localPosition.y";

    [SerializeField, Header("���b�v���[�h")]
    private WrapMode _wapMode = WrapMode.Default;

    [SerializeField, Min(0.0f), Header("�����I������")]
    private float _endTime = 0.0f;

    [SerializeField, Header("�J�n�n�_�̍��W")]
    private Vector2 _startPosition = Vector2.zero;

    [SerializeField, Header("�o�R�A�܂��͏I���n�_�̍��W")]
    public Vector2 _keyPosition = Vector2.zero;

    [SerializeField, Min(0.0f), Header("�o�R�A�܂��͏I���n�_�ɓ��B���鎞��")]
    public float _keyPositionKetTime = 0.0f;

    [Tooltip("���g��Animation")]
    private Animation _myAnimation = null;

    [Tooltip("�쐬����AnimationClip")]
    private AnimationClip _animationClip = null;

    [Tooltip("AnimationCurve��Linear")]
    private AnimationCurve _linearX = null;

    [Tooltip("AnimationCurve��Linear")]
    private AnimationCurve _linearY = null;

    /// <summary>
    /// �J�n���̏���
    /// </summary>
    public Action StartEvent
    {
        set
        {
            var startEvent = new AnimationEvent()
            {
                time = 0.0f,
                functionName = value.Method.Name
            };
            var endEvent = new AnimationEvent()
            {
                time = _endTime,
                functionName = value.Method.Name
            };

            // AnimationClip���쐬
            _animationClip = (_animationClip == null) ? new AnimationClip() : _animationClip;

            // �J�n���̏�����ݒ�
            _animationClip.AddEvent(startEvent);
            _animationClip.AddEvent(endEvent);
        }
    }

    /// <summary>
    /// �I�����̏���
    /// </summary>
    public Action EndEvent
    {
        set
        {
            var endEvent = new AnimationEvent()
            {
                time = _keyPositionKetTime,
                functionName = value.Method.Name
            };

            // AnimationClip���쐬
            _animationClip = (_animationClip == null) ? new AnimationClip() : _animationClip;

            // �I�����̏�����ݒ�
            _animationClip.AddEvent(endEvent);
        }
    }

    private void Start()
    {
        // RequireComponent
        TryGetComponent(out _myAnimation);

        // AnimationClip���쐬
        _animationClip = (_animationClip == null) ? new AnimationClip() : _animationClip;

        //�A�j���[�V�����N���b�v�P�̂ő��삷�邽�߂̐ݒ�
        _animationClip.legacy = true;

        // �����ړ��̐ݒ���쐬
        //�����i�J�n���ԁA�J�n�l�A�I�����ԁA�I���l�j
        _linearX = AnimationCurve.Linear(0.0f, _startPosition.x, _endTime, _startPosition.x);
        _linearY = AnimationCurve.Linear(0.0f, _startPosition.y, _endTime, _startPosition.y);

        // �L�[�t���[���̐ݒ���쐬
        //�����i���ԁA�l�j
        Keyframe keyX = new Keyframe(_keyPositionKetTime, _keyPosition.x);
        Keyframe keyY = new Keyframe(_keyPositionKetTime, _keyPosition.y);

        // �A�j���[�V�����J�[�u�ɃL�[�t���[����ǉ�
        _linearX.AddKey(keyX);
        _linearY.AddKey(keyY);

        // AnimationCurve��ݒ�
        //�����i�p�X�̎w��A�^�C�v�A���썀�ږ��A�A�j���[�V�����J�[�u�j
        _animationClip.SetCurve("", typeof(Transform), LOCAL_POSITION_X, _linearX);
        _animationClip.SetCurve("", typeof(Transform), LOCAL_POSITION_Y, _linearY);

        // ���b�v���[�h�̐ݒ�
        _animationClip.wrapMode = _wapMode;

        // �A�j���[�V�����ɃA�j���[�V�����N���b�v��g�ݍ���
        // �����i�A�j���[�V�����N���b�v�A���O�j
        _myAnimation.AddClip(_animationClip, CLIP_NAME);
        _myAnimation.Play(CLIP_NAME);
    }

    /// <summary>
    /// �A�j���[�V�������I������
    /// </summary>
    public void DisableAnimation()
    {
        _myAnimation.Stop();
    }
}
