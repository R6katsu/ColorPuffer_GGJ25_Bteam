using UnityEngine;
using System;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// 経由、または終了地点の座標と到達時間
/// </summary>
[Serializable]
public struct KeyFrameInfo
{
    [Header("経由、または終了地点の座標")]
    public Vector2 keyPosition;

    [Min(0.0f), Header("経由、または終了地点に到達する時間")]
    public float keyPositionKetTime;
}

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

    [SerializeField, Header("経由、または終了地点の座標と到達時間の配列")]
    private KeyFrameInfo[] _keyPositions = null;

    [Tooltip("自身のAnimation")]
    private Animation _myAnimation = null;

    [Tooltip("作成したAnimationClip")]
    private AnimationClip _animationClip = null;

    [Tooltip("開始地点の座標")]
    private Vector2 _startPosition = Vector2.zero;

    [Tooltip("AnimationCurveのLinear")]
    private AnimationCurve _linearX = null;

    [Tooltip("AnimationCurveのLinear")]
    private AnimationCurve _linearY = null;

    private void OnEnable()
    {
        // 現在の位置を開始地点に設定
        _startPosition = transform.position;
    }

    private void Start()
    {
        // RequireComponent
        TryGetComponent(out _myAnimation);

        // AnimationClipを作成
        _animationClip = new AnimationClip();

        //アニメーションクリップ単体で操作するための設定
        _animationClip.legacy = true;

        // 直線移動の設定を作成
        //引数（開始時間、開始値、終了時間、終了値）
        _linearX = AnimationCurve.Linear(0.0f, _startPosition.x, _endTime, _startPosition.x);
        _linearY = AnimationCurve.Linear(0.0f, _startPosition.y, _endTime, _startPosition.y);

        // キーフレームの設定を作成
        foreach (var keyPosition in _keyPositions)
        {
            //引数（時間、値）
            Keyframe keyX = new Keyframe(keyPosition.keyPositionKetTime, keyPosition.keyPosition.x);
            Keyframe keyY = new Keyframe(keyPosition.keyPositionKetTime, keyPosition.keyPosition.y);

            // アニメーションカーブにキーフレームを追加
            _linearX.AddKey(keyX);
            _linearY.AddKey(keyY);
        }

        // AnimationCurveを設定
        SetCurve();

        // ラップモードの設定
        _animationClip.wrapMode = _wapMode;

        // アニメーションにアニメーションクリップを組み込む
        // 引数（アニメーションクリップ、名前）
        _myAnimation.AddClip(_animationClip, CLIP_NAME);
        _myAnimation.Play(CLIP_NAME);
    }

    /// <summary>
    /// AnimationCurveを設定
    /// </summary>
    private void SetCurve()
    {
        //引数（パスの指定、タイプ、操作項目名、アニメーションカーブ）
        _animationClip.SetCurve("", typeof(Transform), LOCAL_POSITION_X, _linearX);
        _animationClip.SetCurve("", typeof(Transform), LOCAL_POSITION_Y, _linearY);
    }
}
