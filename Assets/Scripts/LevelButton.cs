using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int level;
    [SerializeField] List<GameObject> stars;
    // Start is called before the first frame update
    void Start()
    {
        int starCount = PlayerPrefs.GetInt("Stars" + level, 0);
        for (int i=0; i<starCount; i++)
        {
            stars[i].SetActive(true);
        }
    }
}
