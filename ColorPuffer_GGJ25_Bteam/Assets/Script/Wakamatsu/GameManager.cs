using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text time;
    [SerializeField]
    private TMP_Text score;
    [SerializeField]
    private float countdownTime = 40f;
    [SerializeField]
    private Player player;

    private int currentScore = 0;

    // Update is called once per frame
    void Update()
    {
        countdownTime -= Time.deltaTime;
        if (countdownTime <= 0)
        {
            countdownTime = 0;
        }
        time.text = Mathf.Ceil(countdownTime).ToString();
        if(player.AddScore())
        {
            currentScore++;
            Debug.Log(currentScore);
            player.AddedScore();
        }
        score.text = currentScore.ToString();
    }
}
