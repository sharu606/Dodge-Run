using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFunction : MonoBehaviour
{
    public void OnForestPlayButton()
    {
        GetComponent<AudioSource>().Play();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Forest");
    }

    public void OnSnowPlayButton()
    {
        GetComponent<AudioSource>().Play();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Snow");
    }

    public void OnQuitButton()
    {
        GetComponent<AudioSource>().Play();  
        Application.Quit();
    }

    public void OnThemeButton()
    {
        GetComponent<AudioSource>().Play();  
        UnityEngine.SceneManagement.SceneManager.LoadScene("Themes");
    }   

    public void OnForestChoose()
    {
        GetComponent<AudioSource>().Play();  
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuForest");
    }

    public void OnSnowChoose()
    {
        GetComponent<AudioSource>().Play();  
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuSnow");
    }

    public void OnButton()
    {
        GetComponent<AudioSource>().Play();  
    }
}
