using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [Tooltip("�X�R�A�e�L�X�g")]
    [SerializeField] private TMP_Text score;
    private int scoreIndex;
    private void Start()
    {
        scoreIndex = GameManager.getscore();
        score.text = scoreIndex.ToString();
    }
}
