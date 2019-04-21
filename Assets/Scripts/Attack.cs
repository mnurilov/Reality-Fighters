using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IHitboxResponder
{
    public int damage;
    public Hitbox hitbox;

    public void attack()
    {
        hitbox.setResponder(this);
        // and do the rest of your attack
    }

    public void collisionedWith(Collider2D collider)
    {
        Hurtbox hurtbox = collider.GetComponent<Hurtbox>();
        hurtbox.StateHit();
        hurtbox?.getHitBy(damage);
    }
}
