using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPoints : MonoBehaviour
{
    public int victoryPoints;

    [SerializeField]
    Text victoryPointsText;

    // Start is called before the first frame update
    void Start()
    {
        victoryPoints = 0;
    }

    // Update is called once per frame
    void Update()
    {
        victoryPointsText.text = victoryPoints.ToString();
    }
}
