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
        se = (sound == "punch") ? 
            Resources.Load("Sounds/"+sound+"-"+rand.Next(1,3)) as AudioClip :
            Resources.Load("Sounds/"+sound) as AudioClip;
        
        aus.clip = se;
        aus.Play();

    }
}
