using UnityEngine;
using System;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// 直線移動のAnimationClipを作成し、再生する（2D）
/// </summary>
[RequireComponent(typeof(Animation))]
public class LinerMoveAnimation2D : MonoBehaviour
{
    [Tooltip("作成したアニメーションクリップの名前")]
    private const string CLIP_NAME = "LinerMoveAnimation2D";

    [Tooltip("localPosition.x")]
    private const string LOCAL_POSITION_X = "localPosition.x";

    [Tooltip("localPosition.y")]
    private const string LOCAL_POSITION_Y = "localPosition.y";

    [SerializeField, Header("ラップモード")]
    private WrapMode _wapMode = WrapMode.Default;

    [SerializeField, Min(0.0f), Header("往復終了時間")]
    private float _endTime = 0.0f;

    [SerializeField, Header("開始地点の座標")]
    private Vector2 _startPosition = Vector2.zero;

    [SerializeField, Header("経由、または終了地点の座標")]
    public Vector2 _keyPosition = Vector2.zero;

    [SerializeField, Min(0.0f), Header("経由、または終了地点に到達する時間")]
    public float _keyPositionKetTime = 0.0f;

    [Tooltip("自身のAnimation")]
    private Animation _myAnimation = null;

    [Tooltip("作成したAnimationClip")]
    private AnimationClip _animationClip = null;

    [Tooltip("AnimationCurveのLinear")]
    private AnimationCurve _linearX = null;

    [Tooltip("AnimationCurveのLinear")]
    private AnimationCurve _linearY = null;

    /// <summary>
    /// 開始時の処理
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

            // AnimationClipを作成
            _animationClip = (_animationClip == null) ? new AnimationClip() : _animationClip;

            // 開始時の処理を設定
            _animationClip.AddEvent(startEvent);
            _animationClip.AddEvent(endEvent);
        }
    }

    /// <summary>
    /// 終了時の処理
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

            // AnimationClipを作成
            _animationClip = (_animationClip == null) ? new AnimationClip() : _animationClip;

            // 終了時の処理を設定
            _animationClip.AddEvent(endEvent);
        }
    }

    private void Start()
    {
        // RequireComponent
        TryGetComponent(out _myAnimation);

        // AnimationClipを作成
        _animationClip = (_animationClip == null) ? new AnimationClip() : _animationClip;

        //アニメーションクリップ単体で操作するための設定
        _animationClip.legacy = true;

        // 直線移動の設定を作成
        //引数（開始時間、開始値、終了時間、終了値）
        _linearX = AnimationCurve.Linear(0.0f, _startPosition.x, _endTime, _startPosition.x);
        _linearY = AnimationCurve.Linear(0.0f, _startPosition.y, _endTime, _startPosition.y);

        // キーフレームの設定を作成
        //引数（時間、値）
        Keyframe keyX = new Keyframe(_keyPositionKetTime, _keyPosition.x);
        Keyframe keyY = new Keyframe(_keyPositionKetTime, _keyPosition.y);

        // アニメーションカーブにキーフレームを追加
        _linearX.AddKey(keyX);
        _linearY.AddKey(keyY);

        // AnimationCurveを設定
        //引数（パスの指定、タイプ、操作項目名、アニメーションカーブ）
        _animationClip.SetCurve("", typeof(Transform), LOCAL_POSITION_X, _linearX);
        _animationClip.SetCurve("", typeof(Transform), LOCAL_POSITION_Y, _linearY);

        // ラップモードの設定
        _animationClip.wrapMode = _wapMode;

        // アニメーションにアニメーションクリップを組み込む
        // 引数（アニメーションクリップ、名前）
        _myAnimation.AddClip(_animationClip, CLIP_NAME);
        _myAnimation.Play(CLIP_NAME);
    }

    /// <summary>
    /// アニメーションを終了する
    /// </summary>
    public void DisableAnimation()
    {
        _myAnimation.Stop();
    }
}
