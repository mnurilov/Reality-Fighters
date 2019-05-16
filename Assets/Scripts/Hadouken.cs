using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hadouken : MonoBehaviour, IHitboxResponder
{
    [SerializeField]
    int damage;

    [SerializeField]
    float stunTime;

    [SerializeField]
    Hitbox hitbox;

    public void attack()
    {
        hitbox.setResponder(this);
        hitbox.startCheckingCollision();
    }

    void Update()
    {
        attack();
    }

    public void collisionedWith(Collider2D collider)
    {
        Hurtbox hurtbox = collider.GetComponent<Hurtbox>();

        if (hurtbox.CheckIfGuarding())
        {
            hurtbox?.getHitBy((damage / 5), 0, "parry");
        }
        else
        {
            hurtbox?.getHitBy(damage, stunTime, "light");
        }

        Destroy(gameObject);
    }
}