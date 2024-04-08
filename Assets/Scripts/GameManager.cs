using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int lastUnlockedLevel;
    private void Awake()
    {
        lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 1);
    }
}
