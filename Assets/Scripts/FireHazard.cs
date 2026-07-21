using UnityEngine; 
using System.Collections; 

public class FireHazard : MonoBehaviour 
{
    public int scoreValue; 
    private LevelManager theLevelManager; 

    void Start() 
    {
        scoreValue = 600; // score added for destroying hazard
        theLevelManager = FindFirstObjectByType<LevelManager>();  //find level manager
    }

    private void OnTriggerEnter2D(Collider2D other) //when entering another trigger hitbox
    {

        PlayerController player = other.GetComponent<PlayerController>();  //find player controller
        if (other.CompareTag("Player")) //compare if other tag is player
        {
            player.Die(); //kills player
        }

        if (other.CompareTag("Hammer Hitbox"))  //compare if other tag is hammer hitbox
        {
            Destroy(gameObject); //destroy this hazard
            theLevelManager.AddScore(scoreValue); //add score to the total score
        }
    }
}