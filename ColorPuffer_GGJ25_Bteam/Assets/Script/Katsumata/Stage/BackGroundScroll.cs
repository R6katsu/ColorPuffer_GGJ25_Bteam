using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BackGroundScroll : MonoBehaviour
{
    [SerializeField] private string texturePropertyName = "_MainTex"; // �e�N�X�`���v���p�e�B��
    [SerializeField] private float speed = 1f; // �I�t�Z�b�g�̈ړ����x
    private float offsetX = 0f; // �I�t�Z�b�g��X�l

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
        // X�����̃I�t�Z�b�g���X�V
        offsetX += speed * Time.deltaTime;

        // 0����1�͈̔͂Ƀ��Z�b�g�i���[�v�����j
        if (offsetX > 1f)
        {
            offsetX -= 1f;
        }

        // �}�e���A���ɃI�t�Z�b�g��ݒ�
        Vector2 offset = new Vector2(offsetX, 0f);
        _myMaterial.SetTextureOffset(texturePropertyName, offset);
    }
}
