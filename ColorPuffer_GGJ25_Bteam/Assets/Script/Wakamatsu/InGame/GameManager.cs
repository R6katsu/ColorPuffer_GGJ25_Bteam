using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Tooltip("制限時間テキスト")]
    [SerializeField] private TMP_Text time;
    [Tooltip("スコアテキスト")]
    [SerializeField] private TMP_Text score;
    [Tooltip("制限時間")]
    [SerializeField] private float countdownTime = 40f;
    [Tooltip("プレイヤー")]
    [SerializeField] private Player player;
    [Tooltip("リザルト変移待機時間")]
    [SerializeField] private float waitTime = 5;
    [Tooltip("シーン変移用")]
    [SerializeField] private SceneChange sceneChange;
    [Tooltip("カメラ")]
    [SerializeField] private Camera mainCamera;
    [Tooltip("カメラ移動速度")]
    [SerializeField] private float cameraMoveSpeed = 5f;

    private int currentScore = 0;
    public static int totalScore;
    private float aditionalTime = 0;
    private bool isStop = false;

    /// <summary>
    /// 停止フラグ
    /// </summary>
    public bool IsStop { get => isStop; }

    // Update is called once per frame
    void Update()
    {
        //時間経過
        countdownTime -= Time.deltaTime;
        if (countdownTime <= 0) //0になったら
        {
            // スクロールを停止
            ScrollUtility.ChangeIsScroll(this.GetType(), !isStop);

            totalScore = currentScore;
            if (mainCamera.orthographic)
            {
                CameraControl();
            }
            mainCamera.transform.position += Vector3.back * cameraMoveSpeed * Time.deltaTime;
            isStop = true;
            //テキスト削除
            time.gameObject.SetActive(false);
            score.gameObject.SetActive(false);
            //待機時間カウント
            aditionalTime += Time.deltaTime;
            if (aditionalTime >= waitTime)//シーン移動
            {
                sceneChange.ChangeScene("ResultScene");
            }

        }
        time.text = Mathf.Ceil(countdownTime).ToString(); //時間テキスト描写
        currentScore += player.score;
        player.score = 0;
        score.text = currentScore.ToString(); //スコアテキスト描写
    }
    private void CameraControl()
    {
        mainCamera.orthographic = false;
        mainCamera.fieldOfView = 60f;
        mainCamera.gameObject.transform.position = new Vector3(0, 0, -4); 
    }
    public static int getscore()
    {
        return totalScore;
    }
    public void IsStart() //起動用（仮）
    {
        isStop = false;
    }
}
