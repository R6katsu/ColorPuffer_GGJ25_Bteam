using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

#if UNITY_EDITOR
#endif

/// <summary>
/// �g�����W�V�����̕⏕
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class TransitionHelper : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _seClips = null;

    [Tooltip("���g��AudioSource")]
    private AudioSource _myAudioSource = null;

    /// <summary>
    /// ���g��AudioSource
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
    /// SE��PlayOneShot�ōĐ�
    /// </summary>
    /// <param name="seNumber">�Đ�����SE�̗v�f�ԍ�</param>
    private void PlaySE(int seNumber)
    {
        MyAudioSource.PlayOneShot(_seClips[seNumber]);
    }

    private void PauseSE()
    {
        // �Đ��I���iPlayOneShot���܂܂��j
        MyAudioSource.Stop();
    }
}
