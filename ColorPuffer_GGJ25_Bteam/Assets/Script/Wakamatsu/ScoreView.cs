using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [Tooltip("スコアテキスト")]
    [SerializeField] private TMP_Text score;
    private int scoreIndex;
    private void Start()
    {
        scoreIndex = GameManager.getscore();
        score.text = scoreIndex.ToString();
    }
}
