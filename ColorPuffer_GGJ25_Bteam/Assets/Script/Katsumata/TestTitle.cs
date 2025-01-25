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

        // ���݂̃V�[���̔ԍ����擾
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ���݂̃V�[���ԍ����C���N�������g
        currentSceneIndex++;

        _bubbleTransition.StartTransition(currentSceneIndex);
    }
}
