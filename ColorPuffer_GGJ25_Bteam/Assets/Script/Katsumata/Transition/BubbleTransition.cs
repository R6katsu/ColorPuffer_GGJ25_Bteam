using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 泡のトランジション
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TransitionHelper))]
public class BubbleTransition : MonoBehaviour, ITransition
{
    [Tooltip("アニメーションの長さの最大値")]
    const float DEFAULT_ANIMATION_LENGTH = 1.0f;

    [Tooltip("自身のインスタンス")]
    private BubbleTransition _instance = null;

    [Tooltip("自身のAnimator")]
    private Animator _myAnimator = null;

    [SerializeField, Header("シーン遷移開始の再生Trigger")]
    private string _transitionStartTriggerName = null;

    [SerializeField, Header("シーン遷移終了の再生Trigger")]
    private string _transitionEndTriggerName = null;

    /// <summary>
    /// シーン遷移のAnimator
    /// </summary>
    private Animator TransitionAnimator
    {
        get
        {
            // nullだった
            if (_myAnimator == null)
            {
                // RequireComponent
                TryGetComponent(out _myAnimator);
            }

            return _myAnimator;
        }
    }

    /// <summary>
    /// トランジションの開始
    /// </summary>
    public void StartTransition(int sceneNumber)
    {
        if (_instance == null)
        {
            _instance = this;

            // シーンを跨いで存在する
            DontDestroyOnLoad(transform.parent.gameObject); // Canvas
            DontDestroyOnLoad(gameObject);
        }

        StartCoroutine(Transition(sceneNumber));
    }

    /// <summary>
    /// トランジション
    /// </summary>
    public IEnumerator Transition(int sceneNumber)
    {
        // トランジションを開始
        yield return StartCoroutine(PlayAnimationAndWait(_transitionStartTriggerName));

        // シーン遷移
        yield return SceneManager.LoadSceneAsync(sceneNumber);

        // トランジションを終了
        yield return StartCoroutine(PlayAnimationAndWait(_transitionEndTriggerName));
    }

    /// <summary>
    /// アニメーションを再生し、終了まで待機する
    /// </summary>
    /// <param name="triggerName">アニメーション再生時のTriggerの名称</param>
    /// <returns></returns>
    public IEnumerator PlayAnimationAndWait(string triggerName)
    {
        // トランジションを終了
        TransitionAnimator.SetTrigger(triggerName);

        // アニメーションの遷移を待つ
        yield return null;

        // アニメーション情報の取得
        var stateInfo = TransitionAnimator.GetCurrentAnimatorStateInfo(0);

        // 正しい長さが取得できるまで待機
        while (stateInfo.length == DEFAULT_ANIMATION_LENGTH)
        {
            yield return null;
            stateInfo = TransitionAnimator.GetCurrentAnimatorStateInfo(0);
        }

        // アニメーション終了まで待機
        var animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);
    }
}
