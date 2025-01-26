using UnityEngine;
using System;
using System.Collections;

#if UNITY_EDITOR
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Experimental.GlobalIllumination;
#endif

/// <summary>
/// ����ꂽ��
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ObstaclesMovementRange))]
[RequireComponent(typeof(LinerMoveAnimation2D))]
[RequireComponent(typeof(LineRenderer))]

public class TrappedFish : MonoBehaviour, IObstacle
{
    [Tooltip("������̂ɕK�v�ȐF")]
    private const ColorType HELPED_COLOR = ColorType.Purple;

    [Tooltip("�ނ�l�̈ʒu�̍���")]
    private static readonly Vector2 _anglerHeight = Vector2.up * 20;

    [SerializeField, Min(0), Header("�~���������̓��_")]
    private int _point = 0;

    [SerializeField, Header("�ނ�l�̈ʒu")]
    private Transform _anglerPoint = null;

    [SerializeField, Header("���g��MeshRenderer")]
    private MeshRenderer _myMeshRenderer = null;

    [SerializeField, Min(0.0f), Header("�����鑬�x")]
    private float _speed = 0.0f;

    [SerializeField, Header("���ʉ��Đ��p�̏��")]
    private PlaySEInfo _playSEInfo = new PlaySEInfo();

    [Tooltip("���g��LineRenderer")]
    private LineRenderer _myLineRenderer = null;

    [Tooltip("���g��LinerMoveAnimation2D")]
    private LinerMoveAnimation2D _myLinerMoveAnimation2D = null;

    private Rigidbody2D _myRigidbody = null;
    private Transform _myTransform = null;

    [Tooltip("������")]
    private bool _isFlight = false;

    /// <summary>
    /// �폜���̏���
    /// </summary>
    public Action DieEvent { get; set; }

    private void OnDisable()
    {
        Dispose();
    }

    public void Dispose()
    {
        DieEvent?.Invoke();
    }

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myLineRenderer);
        TryGetComponent(out _myRigidbody);
        TryGetComponent(out _myLinerMoveAnimation2D);
        TryGetComponent(out _myTransform);

        // �ނ莅�Ƌ����q����
        _myLineRenderer.SetPosition(0, _myTransform.position);
        _myLineRenderer.SetPosition(1, (Vector2)_anglerPoint.position + _anglerHeight);

        // �A�j���[�V�����J�n���A�I�����̏�����ݒ�
        _myLinerMoveAnimation2D.StartEvent = () =>
        {
            var localScale = _myMeshRenderer.transform.localScale;
            localScale.x = 1;
            _myMeshRenderer.transform.localScale = localScale;

            var localPosition = _myMeshRenderer.transform.localPosition;
            localPosition.x = 0.4f;
            _myMeshRenderer.transform.localPosition = localPosition;
        };
        _myLinerMoveAnimation2D.EndEvent = () =>
        {
            var localScale = _myMeshRenderer.transform.localScale;
            localScale.x = -1;
            _myMeshRenderer.transform.localScale = localScale;

            var localPosition = _myMeshRenderer.transform.localPosition;
            localPosition.x = -0.4f;
            _myMeshRenderer.transform.localPosition = localPosition;
        };
    }

    private void OnDestroy()
    {
        Destroy(_anglerPoint.gameObject);
    }

    private void Update()
    {
        if (!ScrollUtility.IsScroll)
        {
            _myLinerMoveAnimation2D.DisableAnimation();
        }

        if (_myLineRenderer == null || _isFlight) { return; }

        _myLineRenderer.SetPosition(0, _myTransform.position);
        _myLineRenderer.SetPosition(1, (Vector2)_anglerPoint.position + _anglerHeight);
    }

    /// <summary>
    /// PL�ɓ���������ނ�j���珕������
    /// </summary>
    public (bool, int) HitObstacle(Player player)
    {
        // HELPED_COLOR����Ȃ���Ώ������Ȃ�
        if (HELPED_COLOR != player.CurrentColorType) { return (false, 0); }
        //�X�R�A���Z�p
        player.HitPoint(HELPED_COLOR);

        // ������ꂽ���̌��ʉ����Đ�
        AudioPlayManager.Instance.PlaySE2D
        (
            (int)_playSEInfo.mySENumber,
            _playSEInfo.minPitch,
            _playSEInfo.maxPitch,
            _playSEInfo.volume
        );

        // �����̃t���O�𗧂Ă�
        _isFlight = true;

        // �A�j���[�V�������I��
        _myLinerMoveAnimation2D.DisableAnimation();

        // �ړ��ʂ�������
        _myRigidbody.velocity = Vector2.zero;

        // ���C��0�ɐݒ�
        _myRigidbody.drag = 0.0f;

        // PL���瓦����
        var dir = transform.position - player.transform.position;
        _myRigidbody.AddForce(dir * _speed, ForceMode2D.Impulse);

        // �ނ莅���t�B�[�h�A�E�g
        StartCoroutine(LineFeedOut());

        return (true, _point);
    }

    /// <summary>
    /// �ނ莅���t�B�[�h�A�E�g
    /// </summary>
    /// <returns></returns>
    private IEnumerator LineFeedOut()
    {
        // �����̃}�e���A������F���擾
        Color startColor = _myLineRenderer.startColor;
        Color endColor = _myLineRenderer.endColor;

        float duration = 0.5f; // ����\�����鎞��
        float elapsedTime = 0f;

        while (elapsedTime < duration && this != null)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // �A���t�@�l�����X�Ɍ���������
            float alpha = Mathf.Lerp(1f, 0f, t);

            // �V�����F��ݒ�
            _myLineRenderer.startColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            _myLineRenderer.endColor = new Color(endColor.r, endColor.g, endColor.b, alpha);

            yield return null;
        }
    }
}
