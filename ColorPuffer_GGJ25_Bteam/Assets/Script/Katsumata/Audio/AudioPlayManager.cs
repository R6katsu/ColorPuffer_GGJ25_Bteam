using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
#endif

/// <summary>
/// �����̍Đ��Ǘ�
/// </summary>
public class AudioPlayManager : SingletonMonoBehaviour<AudioPlayManager>
{
    [SerializeField, Header("�Ō��SE�Ɠ���SE�̑ҋ@����")]
    private float _lastSeSpan = 0.0f;

    [SerializeField]
    private AudioSource _bgmAudioSource = null;

    [SerializeField]
    private AudioSource _seAudioSource = null;

    [SerializeField, Header("BGM�z��")]
    private AudioClip[] _bgmClips = null;

    [SerializeField, Header("SE�z��")]
    private AudioClip[] _seClips = null;

    [Tooltip("�Ō��SE���Đ���������")]
    private Dictionary<int, float> _lastSeTimes = new();

    /// <summary>
    /// BGM�Đ�
    /// </summary>
    /// <param name="num">�����ԍ�</param>
    public void PlayBGM(int num)
    {
        // AudioSource��null�A�܂��͈������z��̗v�f���𒴉߂��Ă���
        if (_bgmAudioSource == null
            || _bgmClips.Length <= num) { return; }

        // �����Đ�
        _bgmAudioSource.clip = _bgmClips[num];
        _bgmAudioSource.Play();
    }

    /// <summary>
    /// BGM�Đ�
    /// </summary>
    public void PauseBGM()
    {
        // AudioSource��null������
        if (_bgmAudioSource == null) { return; }

        // �����ꎞ��~
        _bgmAudioSource.Pause();
    }

    /// <summary>
    /// SE�Đ�
    /// </summary>
    /// <param name="num">�����ԍ�</param>
    /// <param name="position">�Đ�����ʒu</param>
    public void PlaySE(int num, Vector3 position, float minPitch = 1.0f, float maxPitch = 1.0f, float volume = 1.0f)
    {
        // �������z��̗v�f���𒴉߂��Ă���
        if (_seClips.Length <= num) { return; }

        // �܂������Ɋ܂܂�Ă��Ȃ�SE�ԍ�������
        if (!_lastSeTimes.ContainsKey(num))
        {
            _lastSeTimes.Add(num, 0.0f);
        }

        // �O���SE�Đ�����A�ҋ@���Ԃ��o�߂��Ă��Ȃ�
        if (_lastSeTimes[num] + _lastSeSpan >= Time.time) { return; }

        // �����_���ȃs�b�`�͈̔͂�ݒ�
        float randomPitch = Random.Range(minPitch, maxPitch);

        // AudioSource.PlayClipAtPoint�Ƀs�b�`�̍��ڂ�ǉ���������
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = _seClips[num];
        audioSource.spatialBlend = 1.0f;
        audioSource.volume = volume;
        audioSource.pitch = randomPitch; // �s�b�`��ݒ�
        audioSource.Play();
        Object.Destroy(gameObject, _seClips[num].length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));

        // �Ō��SE���Đ��������Ԃ��X�V
        _lastSeTimes[num] = Time.time;
    }

    /// <summary>
    /// 2D��SE�Đ�
    /// </summary>
    /// <param name="num">�����ԍ�</param>
    public void PlaySE2D(int num, float minPitch = 1.0f, float maxPitch = 1.0f, float volume = 1.0f)
    {
        // AudioSource��null�A�܂��͈������z��̗v�f���𒴉߂��Ă���
        if (_seAudioSource == null
            || _seClips.Length <= num) { return; }

        // �܂������Ɋ܂܂�Ă��Ȃ�SE�ԍ�������
        if (!_lastSeTimes.ContainsKey(num))
        {
            _lastSeTimes.Add(num, 0.0f);
        }

        var seAudioSource = _seAudioSource;

        // �O���SE�Đ�����A�ҋ@���Ԃ��o�߂��Ă��Ȃ�
        if (_lastSeTimes[num] + _lastSeSpan >= Time.time) { return; }

        // �����_���ȃs�b�`�͈̔͂�ݒ�
        float randomPitch = Random.Range(minPitch, maxPitch);

        // �s�b�`�������_���l�ɐݒ�
        seAudioSource.pitch = randomPitch;

        // ����
        seAudioSource.volume = volume;

        // SE���Đ�
        seAudioSource.PlayOneShot(_seClips[num]);

        // �Ō��SE���Đ��������Ԃ��X�V
        _lastSeTimes[num] = Time.time;

        // �ݒ�����ɖ߂�
        _seAudioSource = seAudioSource;
    }
}
