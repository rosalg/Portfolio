using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button NewGameButton;
    public Button PlayGameButton;

    public void Start()
    {
        if (!GameManager.instance.IndividualLevelData[0].IsUnlocked)
        {
            NewGameButton.gameObject.SetActive(true);
        } else
        {
            PlayGameButton.gameObject.SetActive(true);
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void LoadLevel(string LevelNum)
    {
        if (GameManager.instance.IndividualLevelData[Int32.Parse(LevelNum)].IsUnlocked)
            SceneManager.LoadScene(LevelNum);
    }

}
