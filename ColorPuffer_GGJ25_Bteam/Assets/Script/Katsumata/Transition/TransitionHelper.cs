using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

#if UNITY_EDITOR
#endif

/// <summary>
/// トランジションの補助
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class TransitionHelper : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _seClips = null;

    [Tooltip("自身のAudioSource")]
    private AudioSource _myAudioSource = null;

    /// <summary>
    /// 自身のAudioSource
    /// </summary>
    private AudioSource MyAudioSource
    {
        get
        {
            if (_myAudioSource == null)
            {
                // RequireComponent
                TryGetComponent(out _myAudioSource);
            }

            return _myAudioSource;
        }
    }

    /// <summary>
    /// SEをPlayOneShotで再生
    /// </summary>
    /// <param name="seNumber">再生するSEの要素番号</param>
    private void PlaySE(int seNumber)
    {
        MyAudioSource.PlayOneShot(_seClips[seNumber]);
    }

    private void PauseSE()
    {
        // 再生終了（PlayOneShotも含まれる）
        MyAudioSource.Stop();
    }
}
