using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medium : MonoBehaviour, IHitboxResponder
{
    //AudioSource punch;

    [SerializeField]
    int damage;

    [SerializeField]
    float stunTime;

    [SerializeField]
    Hitbox hitbox;

    public bool Active;

    public bool CanHit;

    void Start()
    {
        //punch = GetComponent<AudioSource>();
    }

    public void attack()
    {
        if (Active)
        {
            hitbox.setResponder(this);
            hitbox.startCheckingCollision();
        }
        else
        {
            hitbox.stopCheckingCollision();
        }
    }

    void Update()
    {
        attack();
    }

    public void MediumActivateCanHit()
    {
        CanHit = true;
    }

    public void MediumDeactivateCanHit()
    {
        CanHit = false;
    }

    public void collisionedWith(Collider2D collider)
    {
        if (CanHit)
        {
            Hurtbox hurtbox = collider.GetComponent<Hurtbox>();
            if (hurtbox.CheckIfGuarding())
            {
                hurtbox?.getHitBy((damage / 5), 0, "parry");
            }
            else
            {
                hurtbox?.getHitBy(damage, stunTime, "medium");
            }
            CanHit = false;
        }
    }
}