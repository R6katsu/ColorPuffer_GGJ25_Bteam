using System.Collections;

/// <summary>
/// �g�����W�V�����̎���
/// </summary>
public interface ITransition
{
    /// <summary>
    /// �g�����W�V�����̊J�n
    /// </summary>
    public void StartTransition(int sceneNumber);

    /// <summary>
    /// �g�����W�V�����̎���
    /// </summary>
    IEnumerator Transition(int sceneNumber);
}
