using UnityEngine;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// 背景のテクスチャのOffsetをスクロール
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class BackGroundScroll : MonoBehaviour
{
    private const string MAIN_TEX = "_MainTex";
    private const float LOOP_RANGE = 1.0f;

    [SerializeField, Header("テクスチャプロパティ名")] 
    private string _texturePropertyName = MAIN_TEX;

    [SerializeField, Header("オフセットの移動速度")] 
    private float _speed = 0.0f;

    [Tooltip("オフセットのX値")]
    private float _offsetX = 0.0f;

    [Tooltip("自身のMeshRenderer")]
    private MeshRenderer _myMeshRenderer = null;

    [Tooltip("自身のMaterial")]
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

        // X方向のオフセットを更新
        _offsetX += _speed * Time.deltaTime;

        // 0から1の範囲にリセット（ループ）
        if (_offsetX > LOOP_RANGE)
        {
            _offsetX -= LOOP_RANGE;
        }

        // マテリアルにオフセットを設定
        Vector2 offset = new Vector2(_offsetX, 0.0f);
        _myMaterial.SetTextureOffset(_texturePropertyName, offset);
    }
}
