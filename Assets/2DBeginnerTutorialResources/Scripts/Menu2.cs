using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu2 : MonoBehaviour
{
    public GameObject menu2;
    public AudioSource music;
    public bool isStop=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isStop)
        {
           if (Input.GetKeyDown(KeyCode.Escape))
           {
               menu2.SetActive(true);
               isStop = false;
               Time.timeScale = 0;//stop
               music.Pause();
           }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
           {
               menu2.SetActive(false);
               isStop = true;
               Time.timeScale = 1;//continue
               music.Play();
           }
        
    }

    public void Continue()
    {
        menu2.SetActive(false);
        isStop = true;
        Time.timeScale = 1;//continue
        music.Play();
    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;//continue
    }

    public void Quit()
    {
        Application.Quit();
    }

}
