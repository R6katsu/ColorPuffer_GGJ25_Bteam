#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#endif

/// <summary>
/// áŠQ•¨‚ÉÚG‚µ‚½Û‚Ìˆ—‚ğÀ‘•
/// </summary>
public interface IObstacle : IDisposable
{
    /// <summary>
    /// áŠQ•¨‚É“–‚½‚Á‚½
    /// </summary>
    public (bool, int) HitObstacle(Player player);

    /// <summary>
    /// íœ‚Ìˆ—
    /// </summary>
    public Action DieEvent { get; set; }
}
