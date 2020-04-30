using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Text previousHighScoreText;
    public GameObject scrollRectObj;
    public GameObject playBtnObj;
    public GameObject backBtnObj;
    public GameObject[] levelSelectionBtns;
    int highScore;
    private int lvl1, lvl2, lvl3, lvl4, lvl5;

    private void Start()
    {
        PlayerPrefs.SetInt("Level1", 1);
        //PlayerPrefs.SetInt("Level2", 0);
        //PlayerPrefs.SetInt("Level3", 0);
        //PlayerPrefs.SetInt("Level4", 0);
        //PlayerPrefs.SetInt("Level5", 0);
        CheckUnlockedLevels();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        previousHighScoreText.text = "HighScore-> " + highScore.ToString();
    }

    private void CheckUnlockedLevels()
    {
        lvl1 = PlayerPrefs.GetInt("Level1", 0);
        lvl2 = PlayerPrefs.GetInt("Level2", 0);
        lvl3 = PlayerPrefs.GetInt("Level3", 0);
        lvl4 = PlayerPrefs.GetInt("Level4", 0);
        lvl5 = PlayerPrefs.GetInt("Level5", 0);
        Debug.Log("level2-> " + lvl2);
        if (lvl1 == 1)
            levelSelectionBtns[0].SetActive(true);
        if (lvl2 == 1)
            levelSelectionBtns[1].SetActive(true);
        else
            levelSelectionBtns[1].SetActive(false);
        if (lvl3 == 1)
        {
            levelSelectionBtns[2].SetActive(true);
        }
        else
            levelSelectionBtns[2].SetActive(false);
        if (lvl4 == 1)
        {
            levelSelectionBtns[3].SetActive(true);
        }
        else
            levelSelectionBtns[3].SetActive(false);
        if (lvl5 == 1)
        {
            levelSelectionBtns[4].SetActive(true);
        }
        else
            levelSelectionBtns[4].SetActive(false);
    }

    public void ShowLevelSelection()
    {
        playBtnObj.SetActive(false);
        scrollRectObj.SetActive(true);
        backBtnObj.SetActive(true);
    }

    public void CloseScrollRectObj()
    {
        playBtnObj.SetActive(true);
        scrollRectObj.SetActive(false);
        backBtnObj.SetActive(false);
    }

    public void SetLevel(int lvlNumber)
    {
        PlayerPrefs.SetInt("LevelNumber", lvlNumber);
        SceneManager.LoadScene(1);
    }
}
