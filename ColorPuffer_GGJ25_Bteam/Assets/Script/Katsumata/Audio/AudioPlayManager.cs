using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
#endif

/// <summary>
/// 音源の再生管理
/// </summary>
public class AudioPlayManager : SingletonMonoBehaviour<AudioPlayManager>
{
    [SerializeField, Header("最後のSEと同じSEの待機時間")]
    private float _lastSeSpan = 0.0f;

    [SerializeField]
    private AudioSource _bgmAudioSource = null;

    [SerializeField]
    private AudioSource _seAudioSource = null;

    [SerializeField, Header("BGM配列")]
    private AudioClip[] _bgmClips = null;

    [SerializeField, Header("SE配列")]
    private AudioClip[] _seClips = null;

    [Tooltip("最後のSEを再生した時間")]
    private Dictionary<int, float> _lastSeTimes = new();

    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="num">音源番号</param>
    public void PlayBGM(int num)
    {
        // AudioSourceがnull、または引数が配列の要素数を超過していた
        if (_bgmAudioSource == null
            || _bgmClips.Length <= num) { return; }

        // 音源再生
        _bgmAudioSource.clip = _bgmClips[num];
        _bgmAudioSource.Play();
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    public void PauseBGM()
    {
        // AudioSourceがnullだった
        if (_bgmAudioSource == null) { return; }

        // 音源一時停止
        _bgmAudioSource.Pause();
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="num">音源番号</param>
    /// <param name="position">再生する位置</param>
    public void PlaySE(int num, Vector3 position, float minPitch = 1.0f, float maxPitch = 1.0f, float volume = 1.0f)
    {
        // 引数が配列の要素数を超過していた
        if (_seClips.Length <= num) { return; }

        // まだ辞書に含まれていないSE番号だった
        if (!_lastSeTimes.ContainsKey(num))
        {
            _lastSeTimes.Add(num, 0.0f);
        }

        // 前回のSE再生から、待機時間を経過していない
        if (_lastSeTimes[num] + _lastSeSpan >= Time.time) { return; }

        // ランダムなピッチの範囲を設定
        float randomPitch = Random.Range(minPitch, maxPitch);

        // AudioSource.PlayClipAtPointにピッチの項目を追加した処理
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = _seClips[num];
        audioSource.spatialBlend = 1.0f;
        audioSource.volume = volume;
        audioSource.pitch = randomPitch; // ピッチを設定
        audioSource.Play();
        Object.Destroy(gameObject, _seClips[num].length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));

        // 最後のSEを再生した時間を更新
        _lastSeTimes[num] = Time.time;
    }

    /// <summary>
    /// 2DでSE再生
    /// </summary>
    /// <param name="num">音源番号</param>
    public void PlaySE2D(int num, float minPitch = 1.0f, float maxPitch = 1.0f, float volume = 1.0f)
    {
        // AudioSourceがnull、または引数が配列の要素数を超過していた
        if (_seAudioSource == null
            || _seClips.Length <= num) { return; }

        // まだ辞書に含まれていないSE番号だった
        if (!_lastSeTimes.ContainsKey(num))
        {
            _lastSeTimes.Add(num, 0.0f);
        }

        var seAudioSource = _seAudioSource;

        // 前回のSE再生から、待機時間を経過していない
        if (_lastSeTimes[num] + _lastSeSpan >= Time.time) { return; }

        // ランダムなピッチの範囲を設定
        float randomPitch = Random.Range(minPitch, maxPitch);

        // ピッチをランダム値に設定
        seAudioSource.pitch = randomPitch;

        // 音量
        seAudioSource.volume = volume;

        // SEを再生
        seAudioSource.PlayOneShot(_seClips[num]);

        // 最後のSEを再生した時間を更新
        _lastSeTimes[num] = Time.time;

        // 設定を元に戻す
        _seAudioSource = seAudioSource;
    }
}
