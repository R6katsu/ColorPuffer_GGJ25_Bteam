using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{

    [SerializeField, Min(0.0f), Header("���b�Z�[�W���\�������܂ł̎���")]
    private float _messageFeedInSeconds = 0.0f;

    [SerializeField, Header("���b�Z�[�W")]
    private Image _messageImage = null;

    [SerializeField, Header("���ʉ��Đ��p�̏��")]
    private PlaySEInfo _playSEInfo = new PlaySEInfo();

    [Tooltip("��������")]
    private bool _isCompleted = false;

    [SerializeField] private SceneChange _sceneChange;
    [SerializeField] private GameManager _gameManager;

    // Button�������ĊJ�n�̕����𐔕b�����ăt�B�[�h�C��������
    // ���̌�A���͂��󂯕t����

    private IEnumerator Start()
    {
        var elapsed = 0.0f;

        // �����̃}�e���A������F���擾
        var color = _messageImage.color;

        // �t�B�[�h�C�����I������܂őҋ@
        while (elapsed <= _messageFeedInSeconds)
        {
            yield return null;

            elapsed += Time.deltaTime;

            // �A���t�@�l�����X�Ɍ���������
            float t = elapsed / _messageFeedInSeconds;
            float alpha = Mathf.Lerp(0.0f, 1.0f, t);

            // ���b�Z�[�W���t�B�[�h�C��
            _messageImage.color = new Color(color.r, color.g, color.b, alpha);
        }

        _isCompleted = true;
    }

    private void Update()
    {
        if (!_isCompleted) { return; }

        // �Ȃɂ�����{�^����������
        if (Input.anyKeyDown)
        {
            ScrollUtility.ChangeIsScroll(this.GetType(), true);

            _isCompleted = false;

            // �{�^�����͎��̌��ʉ����Đ�
            AudioPlayManager.Instance.PlaySE2D
            (
                (int)_playSEInfo.mySENumber,
                _playSEInfo.minPitch,
                _playSEInfo.maxPitch,
                _playSEInfo.volume
            );
            _sceneChange.ChangeScene("TitleScene");
        }
    }
}
