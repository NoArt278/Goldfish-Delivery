using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] List<Image> stars;
    [SerializeField] GameObject allUI;
    [SerializeField] Button nextButton;
    [SerializeField] TMP_Text finishText;

    private void OnEnable()
    {
        StartCoroutine(SlideIn());
    }

    public void SetStars(int count)
    {
        int currLevel = SceneManager.GetActiveScene().buildIndex;
        for (int i = 0; i < count; i++)
        {
            stars[i].color = Color.white;
        }
        if (count < 2) // Failed the level
        {
            finishText.text = "Level failed";
            nextButton.gameObject.SetActive(false);
        } else
        {
            // Unlock level if succeeded
            int lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 1);
            if (currLevel < 6 && currLevel == lastUnlockedLevel)
            {
                PlayerPrefs.SetInt("LastUnlockedLevel", currLevel+1);
            }
        }
        // Set star count
        int lvlStarCount = PlayerPrefs.GetInt("Stars" + currLevel, 1);
        if (count > lvlStarCount)
        {
            PlayerPrefs.SetInt("Stars" + currLevel, count);
        }
    }

    public void NextLevel()
    {
        int currSceneIdx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneIdx + 1);
    }

    public void BackToStart()
    {
        SceneManager.LoadScene(0);
    }    

    public void Retry()
    {
        int currSceneIdx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneIdx);
    }

    IEnumerator SlideIn()
    {
        while(allUI.transform.localPosition.y < 0)
        {
            allUI.transform.localPosition = Vector3.MoveTowards(allUI.transform.localPosition, Vector3.zero, 600 * Time.deltaTime);
            yield return null;
        }
        allUI.transform.localPosition = Vector3.zero;
    }
}
