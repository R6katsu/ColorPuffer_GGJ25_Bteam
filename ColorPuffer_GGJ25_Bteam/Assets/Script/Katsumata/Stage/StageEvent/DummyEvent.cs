using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DummyEvent : MonoBehaviour, IStageEvent
{
    /// <summary>
    /// �C�x���g�̒���
    /// </summary>
    public int EventLength { get => 10; }

    /// <summary>
    /// �C�x���g����������m��
    /// </summary>
    public int EventProbability { get => 30; }

    /// <summary>
    /// �X�e�[�W�Ŕ�������C�x���g
    /// </summary>
    public IEnumerator StageEvent(StageManager stageManager) { yield break; }
}
