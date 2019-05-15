using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingCamera : MonoBehaviour
{
    private Transform[] playerTransforms;

    public float yOffset;
    public float minDistance;

    public bool MaximumDistance = false;

    private float xMin, xMax, yMin, yMax;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        playerTransforms = new Transform[allPlayers.Length];
        for(int i = 0; i < allPlayers.Length; i++)
        {
            playerTransforms[i] = allPlayers[i].transform;
        }
        for(int i = 0; i < allPlayers.Length; i++)
        {
            Debug.Log(playerTransforms[i].ToString());
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(playerTransforms.Length == 0)
        {
            Debug.Log("Have not found a player, make sure player tag is on");
            return;
        }

        xMin = xMax = playerTransforms[0].position.x;
        yMin = yMax = playerTransforms[0].position.y;
        
        for(int i = 1; i < playerTransforms.Length; i++)
        {
            if(playerTransforms[i].position.x < xMin)
            {
                xMin = playerTransforms[i].position.x;
            }
            if (playerTransforms[i].position.x > xMax)
            {
                xMax = playerTransforms[i].position.x;
            }
            if (playerTransforms[i].position.y < yMin)
            {
                yMin = playerTransforms[i].position.y;
            }
            if (playerTransforms[i].position.y > yMax)
            {
                yMax = playerTransforms[i].position.y;
            }
        }

        float xMiddle = (xMin + xMax) / 2;
        
        float distance = Mathf.Abs(xMax - xMin);

        if(distance >= minDistance)
        {
            MaximumDistance = true;
        }
        else
        {
            MaximumDistance = false;
        }

        transform.position = new Vector3(xMiddle, yOffset, -1);
    }
}
