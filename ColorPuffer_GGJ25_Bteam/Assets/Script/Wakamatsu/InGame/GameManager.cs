using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Tooltip("�������ԃe�L�X�g")]
    [SerializeField] private TMP_Text time;
    [Tooltip("�X�R�A�e�L�X�g")]
    [SerializeField] private TMP_Text score;
    [Tooltip("��������")]
    [SerializeField] private float countdownTime = 40f;
    [Tooltip("�v���C���[")]
    [SerializeField] private Player player;
    [Tooltip("���U���g�ψڑҋ@����")]
    [SerializeField] private float waitTime = 5;
    [Tooltip("�V�[���ψڗp")]
    [SerializeField] private SceneChange sceneChange;
    [Tooltip("�J����")]
    [SerializeField] private Camera mainCamera;
    [Tooltip("�J�����ړ����x")]
    [SerializeField] private float cameraMoveSpeed = 5f;

    private int currentScore = 0;
    public static int totalScore;
    private float aditionalTime = 0;
    private bool isStop = false;

    /// <summary>
    /// ��~�t���O
    /// </summary>
    public bool IsStop { get => isStop; }

    // Update is called once per frame
    void Update()
    {
        //���Ԍo��
        countdownTime -= Time.deltaTime;
        if (countdownTime <= 0) //0�ɂȂ�����
        {
            // �X�N���[�����~
            ScrollUtility.ChangeIsScroll(this.GetType(), !isStop);

            totalScore = currentScore;
            if (mainCamera.orthographic)
            {
                CameraControl();
            }
            mainCamera.transform.position += Vector3.back * cameraMoveSpeed * Time.deltaTime;
            isStop = true;
            //�e�L�X�g�폜
            time.gameObject.SetActive(false);
            score.gameObject.SetActive(false);
            //�ҋ@���ԃJ�E���g
            aditionalTime += Time.deltaTime;
            if (aditionalTime >= waitTime)//�V�[���ړ�
            {
                sceneChange.ChangeScene("ResultScene");
            }

        }
        time.text = Mathf.Ceil(countdownTime).ToString(); //���ԃe�L�X�g�`��
        currentScore += player.score;
        player.score = 0;
        score.text = currentScore.ToString(); //�X�R�A�e�L�X�g�`��
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
    public void IsStart() //�N���p�i���j
    {
        isStop = false;
    }
}
