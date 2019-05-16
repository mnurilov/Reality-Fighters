using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour, IHitboxResponder
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
        // and do the rest of your attack
    }

    void Update()
    {
        attack();
    }

    public void LightActivateCanHit()
    {
        CanHit = true;
    }

    public void LightDeactivateCanHit()
    {
        CanHit = false;
    }

    public void collisionedWith(Collider2D collider)
    {
        if (CanHit)
        {
            Hurtbox hurtbox = collider.GetComponent<Hurtbox>();
            //hurtbox.StateHit();
            hurtbox?.getHitBy(damage, stunTime);
            CanHit = false;
            //GetComponent<SoundEffects>().PlaySound("punch");

        }
    }
}