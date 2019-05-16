using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundEffects : MonoBehaviour
{
    System.Random rand;
    AudioSource aus;
    void Start()
    {
        rand = new System.Random();
        aus = GetComponent<AudioSource>();
    }

    public void PlaySound(string sound){
        AudioClip se;
        if(sound == "punch") 
        {
            se = Resources.Load("Sounds/" + sound + "-" + rand.Next(1, 3)) as AudioClip;
        }
        else if(sound == "hadouken")
        {
            se = Resources.Load("Sounds/" + sound) as AudioClip;
        }
        else
        {
            se = Resources.Load("Sounds/" + sound) as AudioClip;
        }

        aus.clip = se;
        aus.Play();
        //aus.PlayOneShot(se, 1f);
    }
}
