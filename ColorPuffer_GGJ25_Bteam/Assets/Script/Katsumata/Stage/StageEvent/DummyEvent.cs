using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DummyEvent : MonoBehaviour, IStageEvent
{
    /// <summary>
    /// イベントの長さ
    /// </summary>
    public int EventLength { get => 10; }

    /// <summary>
    /// イベントが発生する確率
    /// </summary>
    public int EventProbability { get => 30; }

    /// <summary>
    /// ステージで発生するイベント
    /// </summary>
    public IEnumerator StageEvent(StageManager stageManager) { yield break; }
}
