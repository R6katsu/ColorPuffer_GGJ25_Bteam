using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{

    [SerializeField, Min(0.0f), Header("メッセージが表示されるまでの時間")]
    private float _messageFeedInSeconds = 0.0f;

    [SerializeField, Header("メッセージ")]
    private Image _messageImage = null;

    [SerializeField, Header("効果音再生用の情報")]
    private PlaySEInfo _playSEInfo = new PlaySEInfo();

    [Tooltip("準備完了")]
    private bool _isCompleted = false;

    [SerializeField] private SceneChange _sceneChange;
    [SerializeField] private GameManager _gameManager;

    // Buttonを押して開始の文字を数秒かけてフィードインさせる
    // その後、入力を受け付ける

    private IEnumerator Start()
    {
        var elapsed = 0.0f;

        // 既存のマテリアルから色を取得
        var color = _messageImage.color;

        // フィードインが終了するまで待機
        while (elapsed <= _messageFeedInSeconds)
        {
            yield return null;

            elapsed += Time.deltaTime;

            // アルファ値を徐々に減少させる
            float t = elapsed / _messageFeedInSeconds;
            float alpha = Mathf.Lerp(0.0f, 1.0f, t);

            // メッセージをフィードイン
            _messageImage.color = new Color(color.r, color.g, color.b, alpha);
        }

        _isCompleted = true;
    }

    private void Update()
    {
        if (!_isCompleted) { return; }

        // なにかしらボタンを押した
        if (Input.anyKeyDown)
        {
            ScrollUtility.ChangeIsScroll(this.GetType(), true);

            _isCompleted = false;

            // ボタン入力時の効果音を再生
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
