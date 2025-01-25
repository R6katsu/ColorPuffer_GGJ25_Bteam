using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestTitle : MonoBehaviour
{
    [SerializeField]
    private BubbleTransition _bubbleTransition = null;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);

        // 現在のシーンの番号を取得
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 現在のシーン番号をインクリメント
        currentSceneIndex++;

        _bubbleTransition.StartTransition(currentSceneIndex);
    }
}
