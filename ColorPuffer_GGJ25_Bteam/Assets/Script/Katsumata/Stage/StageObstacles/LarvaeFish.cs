using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ‚à‚Á‚Æ¬‹›I
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class LarvaeFish : MonoBehaviour, IObstacle
{
    [SerializeField, Min(0), Header("‹~•¬Œ÷‚Ì“¾“_")]
    private int _point = 0;

    [SerializeField, Min(0.0f), Header("‚«”ò‚Ô‘¬“x")]
    private float _speed = 0.0f;

    [Tooltip("©g‚ÌRigidbody2D")]
    private Rigidbody2D _myRigidbody = null;

    [SerializeField, Header("Œø‰Ê‰¹Ä¶—p‚Ìî•ñ")]
    private PlaySEInfo _playSEInfo = new PlaySEInfo();

    /// <summary>
    /// íœ‚Ìˆ—
    /// </summary>
    public Action DieEvent { get; set; }

    private void OnDisable()
    {
        Dispose();
    }

    public void Dispose()
    {
        DieEvent?.Invoke();
    }

    private void OnEnable()
    {
        // RequireComponent
        TryGetComponent(out _myRigidbody);
    }

    /// <summary>
    /// PL‚É“–‚½‚Á‚½‚ç‚«”ò‚Ô
    /// </summary>
    public (bool, int) HitObstacle(Player player)
    {
        // PL‚ª–c‚ç‚ñ‚Å‚¢‚È‚¯‚ê‚Î•‚¯‚ç‚ê‚é
        var isSuccess = !player.IsBigPuffer;

        // •‚¯‚ç‚ê‚éê‡‚Ì‚İŒø‰Ê‰¹‚ğ–Â‚ç‚·
        if (isSuccess)
        {
            // ‚«”ò‚Ô‚ÌŒø‰Ê‰¹‚ğÄ¶
            AudioPlayManager.Instance.PlaySE2D
            (
                (int)_playSEInfo.mySENumber,
                _playSEInfo.minPitch,
                _playSEInfo.maxPitch,
                _playSEInfo.volume
            );
        }

        // ˆÚ“®—Ê‚ğ‰Šú‰»
        _myRigidbody.velocity = Vector2.zero;

        // –€C–³Œø‰»
        _myRigidbody.drag = 0.0f;

        // ‰ñ“]‚·‚é
        _myRigidbody.AddTorque(_speed, ForceMode2D.Impulse);

        // PL‚©‚ç“¦‚°‚é
        var dir = transform.position - player.transform.position;
        _myRigidbody.AddForce(dir * _speed, ForceMode2D.Impulse);

        return (isSuccess, _point);
    }
}
