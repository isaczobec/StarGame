using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{

    [SerializeField] private string hurtboxTag;
    [SerializeField] private string wallTag;

    
    public class OnHitboxTriggeredEventArgs
    {
        public Collider2D collider;
        public bool hitWall;
        public bool hitHurtbox;

    }
    public event EventHandler<OnHitboxTriggeredEventArgs> OnHitObject;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag(wallTag))
        {
            OnHitObject?.Invoke(this, new OnHitboxTriggeredEventArgs { collider = other, hitWall = true});
        }
        else if (other.CompareTag(hurtboxTag))
        {
            OnHitObject?.Invoke(this, new OnHitboxTriggeredEventArgs { collider = other, hitHurtbox = true});
        }
    }
    
}
