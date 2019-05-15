using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    float shakeDuration = 0f;
 
    float shakeMagnitude;
 
    float dampingSpeed;

    Vector3 initialPosition;

    /*
    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    public void SetShakeAndDamping(float shakeMagnitude, float dampingSpeed)
    {
        this.shakeMagnitude = shakeMagnitude;
        this.dampingSpeed = dampingSpeed;
    }

    public void TriggerShake(float shakeDuration)
    {
        initialPosition = transform.localPosition;
        this.shakeDuration = shakeDuration;
    }
}
