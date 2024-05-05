using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitboxTriggeredInfo
{
    public Collider2D collider;
    public bool hitWall;
    public bool hitHurtbox;

}

public class Hitbox : MonoBehaviour
{

    [SerializeField] private string hurtboxTag;
    [SerializeField] private string wallTag;
    [SerializeField] private string effectorTag;

    /// <summary>
    /// the hitbox entity that owns this hitbox
    /// </summary>
    private IHitboxEntity ownerHitboxEntity;

    

    public void SetOwnerHitboxEntity(IHitboxEntity ownerHitboxEntity)
    {
        this.ownerHitboxEntity = ownerHitboxEntity;
    }

    /// <summary>
    /// Check if the player hit a wall, hurtbox or effector, and invokes the corresponding logic
    /// </summary>
    /// <param name="other"></param>

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (CheckIfHitWasHurtbox(other))
        {
            return;
        } else if (CheckIfHitWasEffector(other))
        {
            return;
        }
        
    }

    /// <summary>
    /// Check if the hit was a hurtbox or a wall and invoke the event if it was
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    private bool CheckIfHitWasHurtbox(Collider2D other)
    {
        if (other.CompareTag(DefaultTagNames.wallTag))
        {
            ownerHitboxEntity.OnHurtBoxHit(new HitboxTriggeredInfo { collider = other, hitWall = true});
            return true;
        }
        else if (other.CompareTag(DefaultTagNames.hurtboxTag))
        {
            ownerHitboxEntity.OnHurtBoxHit(new HitboxTriggeredInfo { collider = other, hitHurtbox = true});
            return true;
        }
        return false;
    }

    /// <summary>
    /// Check if the hit was an effector and run the effector logic
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    private bool CheckIfHitWasEffector(Collider2D other)
    {
        if (other.CompareTag(DefaultTagNames.effectorTag))
        {
            Effector effector = other.GetComponent<Effector>();
            effector.OnEffectorTriggered(ownerHitboxEntity);
            return true;
        }
        return false;
    }


    
}
