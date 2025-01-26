using System.Collections;
using UnityEngine;

/// <summary>
/// 大量発生イベント
/// </summary>
public class MassOutbreak : MonoBehaviour, IStageEvent
{
    [Tooltip("アニメーションの長さの最大値")]
    const float DEFAULT_ANIMATION_LENGTH = 1.0f;

    [SerializeField, Min(0),  Header("イベントが発生する確率")]
    private int _eventProbability = 0;

    [SerializeField, Min(0), Header("イベントの長さ")]
    private int _eventLength = 0;

    [SerializeField, Min(0.0f), Header("障害物を生成する間隔")]
    private float _overrideObstacleSpawnSpan = 0.0f;

    [SerializeField, Header("大量発生させる障害物")]
    private Transform[] _overrideObstaclePrefabs = null;

    [SerializeField, Header("イベントの名称を書いたアニメーション")]
    private Animator _eventNameAnimator = null;

    [SerializeField, Header("シーン遷移開始の再生Trigger")]
    private string _startTriggerName = null;

    [SerializeField, Header("シーン遷移終了の再生Trigger")]
    private string _endTriggerName = null;

    /// <summary>
    /// イベントの長さ
    /// </summary>
    public int EventLength { get => _eventLength; }

    /// <summary>
    /// イベントが発生する確率
    /// </summary>
    public int EventProbability { get => _eventProbability; }

    /// <summary>
    /// ステージで発生するイベント
    /// </summary>
    public IEnumerator StageEvent(StageManager stageManager)//EndTrigger
    {
        // 障害物を生成する間隔を上書きする
        stageManager.OverrideObstacleSpawnSpan(_overrideObstacleSpawnSpan);

        // 生成する障害物を上書きする
        stageManager.OverrideObstaclePrefabs(_overrideObstaclePrefabs);

        // 開始アニメーション再生
        StartCoroutine(PlayAnimationAndWait(_startTriggerName));

        // イベント終了まで待機
        yield return new WaitForSeconds(EventLength);

        // 終了アニメーション再生
        StartCoroutine(PlayAnimationAndWait(_endTriggerName));

        // 生成感覚を初期化
        stageManager.ResetOverrideObstacleSpawnSpan();

        // 生成する障害物を初期化
        stageManager.ResetObstaclePrefabs();
    }

    /// <summary>
    /// アニメーションを再生し、終了まで待機する
    /// </summary>
    /// <param name="triggerName">アニメーション再生時のTriggerの名称</param>
    /// <returns></returns>
    public IEnumerator PlayAnimationAndWait(string triggerName)
    {
        // トランジションを終了
        _eventNameAnimator.SetTrigger(triggerName);

        // アニメーションの遷移を待つ
        yield return null;

        // アニメーション情報の取得
        var stateInfo = _eventNameAnimator.GetCurrentAnimatorStateInfo(0);

        // 正しい長さが取得できるまで待機
        while (stateInfo.length == DEFAULT_ANIMATION_LENGTH)
        {
            yield return null;
            stateInfo = _eventNameAnimator.GetCurrentAnimatorStateInfo(0);
        }

        // アニメーション終了まで待機
        var animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);
    }
}
