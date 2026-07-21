using UnityEngine; 
using System.Collections; 

public class FireHazard : MonoBehaviour 
{
    public int scoreValue; 
    private LevelManager theLevelManager; 

    void Start() 
    {
        theLevelManager = FindFirstObjectByType<LevelManager>(); 
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {

        PlayerController player = other.GetComponent<PlayerController>(); 
        if (other.CompareTag("Player"))
        {
            player.Die();
        }

        if (other.CompareTag("Hammer Hitbox")) 
        {
            Destroy(gameObject);
            theLevelManager.AddScore(scoreValue); 
        }
    }
}