using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string startScene;
    public void StartGame()
    {
        SceneManager.LoadScene(startScene);

    }

    public void OnApplicationQuit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
    
    
}
