using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using TMPro;
using Unity.VisualScripting;
#endif

/// <summary>
/// ¬‹›
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Fish : MonoBehaviour, IObstacle
{
    [SerializeField, Min(0.0f), Header("ˆÚ“®‘¬“x")]
    private float _speed = 0.0f;

    private Rigidbody2D _myRigidbody = null;

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myRigidbody);
    }

    /// <summary>
    /// PL‚É“–‚½‚Á‚½‚çPL‚©‚ç“¦‚°‚é
    /// </summary>
    public void HitObstacle(/* Player player */Transform player)
    {
        // ˆÚ“®—Ê‚ğ‰Šú‰»
        _myRigidbody.velocity = Vector2.zero;

        // PL‚©‚ç“¦‚°‚é
        var dir = transform.position - player.transform.position;
        _myRigidbody.AddForce(dir * _speed, ForceMode2D.Impulse);
    }
}
