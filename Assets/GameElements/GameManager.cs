using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text blueScoreText, redScoreText;

    private int redScore=0, blueScore=0;

    private void Update()
    {
        blueScoreText.text = "" + blueScore;    
        redScoreText.text = "" + redScore;    
    }

    //redScore must be true when adding to red alliance score.
    public void AddToScore(int points, bool addingToRed)
    {
        if (addingToRed)
        {
            redScore += points;
        }
        else
        {
            blueScore += points;
        }
    }
}
