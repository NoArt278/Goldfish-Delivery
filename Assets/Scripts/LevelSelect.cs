using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] List<Button> levelsButton;
    [SerializeField] GameObject startHUD;

    private void Start()
    {
        int lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 1);
        for (int i=0; i<lastUnlockedLevel; i++)
        {
            levelsButton[i].interactable = true;
        }
    }
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void BackToStart()
    {
        startHUD.SetActive(true);
        gameObject.SetActive(false);
    }
}
