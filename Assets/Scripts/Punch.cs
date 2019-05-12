using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Punch : MonoBehaviour, IHitboxResponder
{
    public AudioClip punch_1;

    public AudioClip punch_2;

    AudioSource punch;

    System.Random rand;

    [SerializeField]
    int damage;

    [SerializeField]
    float stunTime;

    [SerializeField]
    Hitbox hitbox;

    Hurtbox cacheHurtbox;

    public bool Active;

    public bool CanHit;

    void Start(){
        punch = GetComponent<AudioSource>();
        rand = new System.Random();

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

    public void ActivateCanHit()
    {
        CanHit = true;
    }

    public void collisionedWith(Collider2D collider)
    {
        if (CanHit)
        {
            int rand_punch = rand.Next(0,2);

            punch.clip = (rand_punch == 1) ? punch_1 : punch_2;
            punch.Play();

            Hurtbox hurtbox = collider.GetComponent<Hurtbox>();
           //hurtbox.StateHit();
            hurtbox?.getHitBy(damage, stunTime);
            CanHit = false;

            
        }
    }
}