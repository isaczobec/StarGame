using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for entities that have hitboxes
/// </summary>
public interface IHitboxEntity
{
    public void OnHurtBoxHit(HitboxTriggeredInfo hitboxTriggeredInfo);
    public bool isPlayer();
}
