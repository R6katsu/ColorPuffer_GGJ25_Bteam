#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endif

/// <summary>
/// ステージイベントを実装
/// </summary>
public interface IStageEvent
{
    #region イベントの例
    // 島接近注意！
    // 画面下半分が地面となり、移動範囲が画面上半分に制限される

    // 海の森突入！
    // 海藻（わかめ）の群生地帯。視界を妨害する背の高いわかめが近景として流れるようになる

    // 異常気象だ！
    // 泡と死にかけの魚のみが出現するようになる

    // 一本釣りだ！
    // 泡と釣られている魚のみが出現するようになる

    // もっと小魚！
    // 小魚より小さな小魚が出現するようになる。小魚より小さいため、当たってもダメージを受けない。
    // 膨らんでいない状態で当たると得点になり、膨らんでいる状態で当たると減点になる（傷つけると減点）。色は関係ない
    #endregion

    /// <summary>
    /// イベントが発生する確率
    /// </summary>
    public int EventProbability { get; }

    /// <summary>
    /// イベントの長さ
    /// </summary>
    public int EventLength { get; }

    /// <summary>
    /// ステージで発生するイベント
    /// </summary>
    public IEnumerator StageEvent(StageManager stageManager);

    // StageManagerを渡して、好き勝手処理して貰う？
    // StageManager側でいじれる値を用意しておき、StageEventの関数内でいじる
    // もっと小魚に登場する魚のPrefabなど、インスペクタから設定が必要なものがある
    // IStageEventを継承したclassのインスペクタから設定する。
    // また、シーンに存在するIStageEventを継承した全てのclassを最初に取得する
    // この時、イベントが発生する確率もインターフェースで実装しておく
    // イベントが発生している時、別のイベントが発生しないようにする
}
