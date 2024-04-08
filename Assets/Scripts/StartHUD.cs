using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartHUD : MonoBehaviour
{
    [SerializeField] GameObject levelSelect;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LevelSelect()
    {
        levelSelect.SetActive(true);
        gameObject.SetActive(false);
    }
}
