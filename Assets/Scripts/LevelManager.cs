using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class LevelManager : MonoBehaviour
{
    public int scoreCount;
    public TextMeshProUGUI scoreText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (scoreText != null)
            scoreText.text = "Score:" + scoreCount;


    }
        // Update is called once per frame
        
    public void AddScore(int scoreToAdd)
    {
        scoreCount += scoreToAdd;
        if (scoreText != null)
            scoreText.text = "Score:" + scoreCount;
    }
}