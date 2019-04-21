using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    GameObject bar;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the bar game object which is a child of the healthbar
        bar = transform.GetChild(2).gameObject;
    }

    public void SetSize(float sizeNormalized)
    {
        //Debug.Log(sizeNormalized);
        bar.transform.localScale = new Vector3(sizeNormalized, 1f);
    }
}
