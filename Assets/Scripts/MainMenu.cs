using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //When click on a button, will take player to to the corresponding scene.. or just quit the game 
    public void NewGame()
    {
        SceneManager.LoadScene("Level Select"); //loads level selector
    }

    public void Credits() 
    {
        SceneManager.LoadScene("Credits"); //loads credits
    }

    public void QuitGame()
    {
        Application.Quit(); //leaves game
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Main Menu"); //loads main menu
    }
    public void Easy()
    {
        SceneManager.LoadScene("Level 01"); //loads level 1
    }
    public void Medium()
    {
        SceneManager.LoadScene("Level 02"); //loads level 2
    }
    public void Hard()
    {
        SceneManager.LoadScene("Level 03"); //loads level 3
    }

}
