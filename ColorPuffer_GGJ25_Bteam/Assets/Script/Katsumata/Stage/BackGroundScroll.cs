using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// �w�i�̃e�N�X�`����Offset���X�N���[��
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class BackGroundScroll : MonoBehaviour
{
    private const string MAIN_TEX = "_MainTex";
    private const float LOOP_RANGE = 1.0f;

    [SerializeField, Header("�e�N�X�`���v���p�e�B��")] 
    private string _texturePropertyName = MAIN_TEX;

    [SerializeField, Header("�I�t�Z�b�g�̈ړ����x")] 
    private float _speed = 0.0f;

    [Tooltip("�I�t�Z�b�g��X�l")]
    private float _offsetX = 0.0f;

    [Tooltip("���g��MeshRenderer")]
    private MeshRenderer _myMeshRenderer = null;

    [Tooltip("���g��Material")]
    private Material _myMaterial = null;

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myMeshRenderer);
        _myMaterial = _myMeshRenderer.material;
    }

    private void Update()
    {
        if (!ScrollUtility.IsScroll) { return; }

        // X�����̃I�t�Z�b�g���X�V
        _offsetX += _speed * Time.deltaTime;

        // 0����1�͈̔͂Ƀ��Z�b�g�i���[�v�j
        if (_offsetX > LOOP_RANGE)
        {
            _offsetX -= LOOP_RANGE;
        }

        // �}�e���A���ɃI�t�Z�b�g��ݒ�
        Vector2 offset = new Vector2(_offsetX, 0.0f);
        _myMaterial.SetTextureOffset(_texturePropertyName, offset);
    }
}
