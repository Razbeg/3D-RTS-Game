using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    public static EndScreenUI Instance;
    
    public GameObject endScreen;
    public GameObject winText;
    public GameObject loseText;

    private void Awake()
    {
        Instance = this;
    }

    public void SetEndScreen(bool win)
    {
        endScreen.SetActive(true);
        
        winText.SetActive(win);
        loseText.SetActive(!win);
    }

    public void OnPlayAgainButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
