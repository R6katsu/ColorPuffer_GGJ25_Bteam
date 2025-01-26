using UnityEngine;
using System;
using System.Collections;

#if UNITY_EDITOR
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Experimental.GlobalIllumination;
#endif

/// <summary>
/// 囚われた魚
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ObstaclesMovementRange))]
[RequireComponent(typeof(LinerMoveAnimation2D))]
[RequireComponent(typeof(LineRenderer))]

public class TrappedFish : MonoBehaviour, IObstacle
{
    [Tooltip("助けるのに必要な色")]
    private const ColorType HELPED_COLOR = ColorType.Purple;

    [Tooltip("釣り人の位置の高さ")]
    private static readonly Vector2 _anglerHeight = Vector2.up * 20;

    [SerializeField, Min(0), Header("救助成功時の得点")]
    private int _point = 0;

    [SerializeField, Header("釣り人の位置")]
    private Transform _anglerPoint = null;

    [SerializeField, Header("自身のMeshRenderer")]
    private MeshRenderer _myMeshRenderer = null;

    [SerializeField, Min(0.0f), Header("逃げる速度")]
    private float _speed = 0.0f;

    [SerializeField, Header("効果音再生用の情報")]
    private PlaySEInfo _playSEInfo = new PlaySEInfo();

    [Tooltip("自身のLineRenderer")]
    private LineRenderer _myLineRenderer = null;

    [Tooltip("自身のLinerMoveAnimation2D")]
    private LinerMoveAnimation2D _myLinerMoveAnimation2D = null;

    private Rigidbody2D _myRigidbody = null;
    private Transform _myTransform = null;

    [Tooltip("逃走中")]
    private bool _isFlight = false;

    /// <summary>
    /// 削除時の処理
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

        // 釣り糸と魚を繋げる
        _myLineRenderer.SetPosition(0, _myTransform.position);
        _myLineRenderer.SetPosition(1, (Vector2)_anglerPoint.position + _anglerHeight);

        // アニメーション開始時、終了時の処理を設定
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
    /// PLに当たったら釣り針から助けられる
    /// </summary>
    public (bool, int) HitObstacle(Player player)
    {
        // HELPED_COLORじゃなければ助けられない
        if (HELPED_COLOR != player.CurrentColorType) { return (false, 0); }
        //スコア加算用
        player.HitPoint(HELPED_COLOR);

        // 助けられた時の効果音を再生
        AudioPlayManager.Instance.PlaySE2D
        (
            (int)_playSEInfo.mySENumber,
            _playSEInfo.minPitch,
            _playSEInfo.maxPitch,
            _playSEInfo.volume
        );

        // 逃走のフラグを立てる
        _isFlight = true;

        // アニメーションを終了
        _myLinerMoveAnimation2D.DisableAnimation();

        // 移動量を初期化
        _myRigidbody.velocity = Vector2.zero;

        // 摩擦を0に設定
        _myRigidbody.drag = 0.0f;

        // PLから逃げる
        var dir = transform.position - player.transform.position;
        _myRigidbody.AddForce(dir * _speed, ForceMode2D.Impulse);

        // 釣り糸をフィードアウト
        StartCoroutine(LineFeedOut());

        return (true, _point);
    }

    /// <summary>
    /// 釣り糸をフィードアウト
    /// </summary>
    /// <returns></returns>
    private IEnumerator LineFeedOut()
    {
        // 既存のマテリアルから色を取得
        Color startColor = _myLineRenderer.startColor;
        Color endColor = _myLineRenderer.endColor;

        float duration = 0.5f; // 線を表示する時間
        float elapsedTime = 0f;

        while (elapsedTime < duration && this != null)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // アルファ値を徐々に減少させる
            float alpha = Mathf.Lerp(1f, 0f, t);

            // 新しい色を設定
            _myLineRenderer.startColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            _myLineRenderer.endColor = new Color(endColor.r, endColor.g, endColor.b, alpha);

            yield return null;
        }
    }
}
