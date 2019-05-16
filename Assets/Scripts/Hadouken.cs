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

    //AudioSource audioSource;

    //SoundEffects soundEffects;

    void Start()
    {
        Debug.Log("HADOUKEN!");
        GetComponent<SoundEffects>().PlaySound("hadouken");

        //GetComponent<SoundEffects>().PlaySound("punch");
        //audioSource = GetComponent<AudioSource>();
        //GetComponent<AudioSource>().Play();
        //soundEffects = GetComponent<SoundEffects>();
    }

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

        }
        else
        {
            hurtbox?.getHitBy(damage, stunTime);
        }
        
        Destroy(gameObject);
    }
}