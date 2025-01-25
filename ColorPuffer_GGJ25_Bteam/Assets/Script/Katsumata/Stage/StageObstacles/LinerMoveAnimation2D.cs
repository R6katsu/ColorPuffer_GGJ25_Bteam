using UnityEngine;
using System;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// �o�R�A�܂��͏I���n�_�̍��W�Ɠ��B����
/// </summary>
[Serializable]
public struct KeyFrameInfo
{
    [Header("�o�R�A�܂��͏I���n�_�̍��W")]
    public Vector2 keyPosition;

    [Min(0.0f), Header("�o�R�A�܂��͏I���n�_�ɓ��B���鎞��")]
    public float keyPositionKetTime;
}

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

    [SerializeField, Header("�o�R�A�܂��͏I���n�_�̍��W�Ɠ��B���Ԃ̔z��")]
    private KeyFrameInfo[] _keyPositions = null;

    [Tooltip("���g��Animation")]
    private Animation _myAnimation = null;

    [Tooltip("�쐬����AnimationClip")]
    private AnimationClip _animationClip = null;

    [Tooltip("�J�n�n�_�̍��W")]
    private Vector2 _startPosition = Vector2.zero;

    [Tooltip("AnimationCurve��Linear")]
    private AnimationCurve _linearX = null;

    [Tooltip("AnimationCurve��Linear")]
    private AnimationCurve _linearY = null;

    private void OnEnable()
    {
        // ���݂̈ʒu���J�n�n�_�ɐݒ�
        _startPosition = transform.position;
    }

    private void Start()
    {
        // RequireComponent
        TryGetComponent(out _myAnimation);

        // AnimationClip���쐬
        _animationClip = new AnimationClip();

        //�A�j���[�V�����N���b�v�P�̂ő��삷�邽�߂̐ݒ�
        _animationClip.legacy = true;

        // �����ړ��̐ݒ���쐬
        //�����i�J�n���ԁA�J�n�l�A�I�����ԁA�I���l�j
        _linearX = AnimationCurve.Linear(0.0f, _startPosition.x, _endTime, _startPosition.x);
        _linearY = AnimationCurve.Linear(0.0f, _startPosition.y, _endTime, _startPosition.y);

        // �L�[�t���[���̐ݒ���쐬
        foreach (var keyPosition in _keyPositions)
        {
            //�����i���ԁA�l�j
            Keyframe keyX = new Keyframe(keyPosition.keyPositionKetTime, keyPosition.keyPosition.x);
            Keyframe keyY = new Keyframe(keyPosition.keyPositionKetTime, keyPosition.keyPosition.y);

            // �A�j���[�V�����J�[�u�ɃL�[�t���[����ǉ�
            _linearX.AddKey(keyX);
            _linearY.AddKey(keyY);
        }

        // AnimationCurve��ݒ�
        SetCurve();

        // ���b�v���[�h�̐ݒ�
        _animationClip.wrapMode = _wapMode;

        // �A�j���[�V�����ɃA�j���[�V�����N���b�v��g�ݍ���
        // �����i�A�j���[�V�����N���b�v�A���O�j
        _myAnimation.AddClip(_animationClip, CLIP_NAME);
        _myAnimation.Play(CLIP_NAME);
    }

    /// <summary>
    /// AnimationCurve��ݒ�
    /// </summary>
    private void SetCurve()
    {
        //�����i�p�X�̎w��A�^�C�v�A���썀�ږ��A�A�j���[�V�����J�[�u�j
        _animationClip.SetCurve("", typeof(Transform), LOCAL_POSITION_X, _linearX);
        _animationClip.SetCurve("", typeof(Transform), LOCAL_POSITION_Y, _linearY);
    }
}
