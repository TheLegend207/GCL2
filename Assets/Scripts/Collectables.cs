using UnityEngine;

public class Collectables : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int scoreValue;
    private LevelManager theLevelManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start
        (


        )
    {
        theLevelManager = FindFirstObjectByType<LevelManager>(); //finds level manager
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D other) //when colliding with another trigger hitbox
    {
        if (other.tag == "Player") //if tag is player
        {
            theLevelManager.AddScore(scoreValue); //add score to the level manager
            Destroy(gameObject); //destroy collectable
        }
    }

}