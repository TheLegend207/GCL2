using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class LevelManager : MonoBehaviour
{
    public int scoreCount;
    public TextMeshProUGUI scoreText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (scoreText != null)
            scoreText.text = "Score:" + scoreCount; //shows the current score


    }
    // Update is called once per frame

    public void AddScore(int scoreToAdd) 
    {
        scoreCount += scoreToAdd; //add score from other scripts to the current score count
        if (scoreText != null)
            scoreText.text = "Score:" + scoreCount; //shows the new current score
    }
}