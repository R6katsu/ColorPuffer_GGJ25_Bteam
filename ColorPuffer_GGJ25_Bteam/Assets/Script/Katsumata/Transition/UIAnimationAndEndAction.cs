using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// アニメーションを再生する。再生が終わるまで待機し、登録された処理を実行する
/// </summary>
public class UIAnimationAndEndAction : SingletonMonoBehaviour<UIAnimationAndEndAction>
{
    [Tooltip("アニメーションの長さの最大値")]
    const float DEFAULT_ANIMATION_LENGTH = 1.0f;

    [Tooltip("自身のインスタンス")]
    private UIAnimationAndEndAction _instance = null;

    public void OnEnable()
    {
        if (_instance == null)
        {
            _instance = this;

            // シーンを跨いで存在する
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// アニメーションを再生する。再生が終わるまで待機し、登録された処理を実行する
    /// </summary>
    /// <param name="animator">再生するAnimator</param>
    /// <param name="triggerName">AnimatorのTrigger変数の名称</param>
    /// <param name="endAction">終了時に実行される処理</param>
    /// <returns></returns>
    public IEnumerator AnimationAndEndAction(Animator animator, string triggerName, Action endAction = null)
    {
        // タイトルロゴを再生
        animator.SetTrigger(triggerName);

        // アニメーションの遷移を待つ
        yield return null;

        // アニメーション情報の取得
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 正しい長さが取得できるまで待機
        while (animator.GetCurrentAnimatorStateInfo(0).length == DEFAULT_ANIMATION_LENGTH)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // アニメーション終了まで待機
        var animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);

        // アニメーション終了時に登録された処理を実行
        endAction?.Invoke();
    }
}