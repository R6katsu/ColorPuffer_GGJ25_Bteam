using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BackGroundScroll : MonoBehaviour
{
    [SerializeField] private string texturePropertyName = "_MainTex"; // テクスチャプロパティ名
    [SerializeField] private float speed = 1f; // オフセットの移動速度
    private float offsetX = 0f; // オフセットのX値

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
        // X方向のオフセットを更新
        offsetX += speed * Time.deltaTime;

        // 0から1の範囲にリセット（ループ処理）
        if (offsetX > 1f)
        {
            offsetX -= 1f;
        }

        // マテリアルにオフセットを設定
        Vector2 offset = new Vector2(offsetX, 0f);
        _myMaterial.SetTextureOffset(texturePropertyName, offset);
    }
}
