using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //When click on a button, will take player to to the corresponding scene.. or just quit the game 
    public void NewGame()
    {
        SceneManager.LoadScene("Level 01");
    }

    public void Credits() 
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
