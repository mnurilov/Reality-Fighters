using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPoint : MonoBehaviour
{
    public bool Active;

    GameObject point;

    // Start is called before the first frame update
    void Start()
    {
        point = transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            point.SetActive(true);
        }
        else
        {
            point.SetActive(false);
        }
    }
}
