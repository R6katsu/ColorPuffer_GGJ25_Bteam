#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#endif

/// <summary>
/// áŠQ•¨‚ÉÚG‚µ‚½Û‚Ìˆ—‚ğÀ‘•
/// </summary>
public interface IObstacle
{
    /// <summary>
    /// áŠQ•¨‚É“–‚½‚Á‚½
    /// </summary>
    public bool HitObstacle(Player player);
}
