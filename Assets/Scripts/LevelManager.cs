using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private List<House> houses;
    private List<Goldfish> goldfishes;
    [SerializeField] private List<Package> packages;
    [SerializeField] private GameObject dialogueBox, HUD;
    [SerializeField] private LevelFinish levelFinish;
    private int correctCount, wrongCount, totalHouse;
    [SerializeField] Transform houseParent, goldfishParent;

    public bool playDialogue;
    public Goldfish selectedGoldfish;
 
    private void Start()
    {
        correctCount = 0;
        wrongCount = 0;
        if (playDialogue)
        {
            dialogueBox.SetActive(true);
            Time.timeScale = 0;
        }
        selectedGoldfish = null;
        houses = new List<House>();
        goldfishes = new List<Goldfish>();

        // Choose package randomly from total list
        List<Package> chosenPackages = new List<Package>();
        for (int i = 0; i < houseParent.childCount; i++)
        {
            int chosenIdx = Random.Range(0, packages.Count);
            chosenPackages.Add(packages[chosenIdx]);
            packages.Remove(packages[chosenIdx]);
        }

        int packageCount = chosenPackages.Count;
        // Assign package randomly to all houses
        List<Package> tempPackages = new List<Package>();
        foreach (Package p in chosenPackages)
        {
            tempPackages.Add(p);
        }
        for (int i=0; i < houseParent.childCount; i++)
        {
            houses.Add(houseParent.GetChild(i).GetComponent<House>());
            houses[i].levelManager = this;
            // Assign random package to each house
            int chosenIdx = Random.Range(0, tempPackages.Count);
            houses[i].SetWantedPackage(tempPackages[chosenIdx]);
            tempPackages.Remove(tempPackages[chosenIdx]);
        }
        totalHouse = houses.Count;

        // Assign package randomly to all goldfish
        for (int i = 0; i < goldfishParent.childCount; i++)
        {
            int endIdx = packageCount / goldfishParent.childCount;
            if (i == goldfishParent.childCount-1)
            {
                // If last fish, assign all remaining packages
                endIdx = chosenPackages.Count;
            }
            goldfishes.Add(goldfishParent.GetChild(i).GetComponent<Goldfish>());
            goldfishes[i].levelManager = this;
            
            for (int j=0; j < endIdx; j++)
            {
                int chosenIdx = Random.Range(0, chosenPackages.Count);
                goldfishes[i].AddPackage(chosenPackages[chosenIdx]);
                chosenPackages.Remove(chosenPackages[chosenIdx]);
            }
        }
    }

    public List<House> GetHouseList()
    {
        return houses;
    }

    public void RemoveHouse(House h)
    {
        houses.Remove(h);
        // Level is finished, all houses have received a mail
        if (houses.Count == 0 )
        {
            levelFinish.gameObject.SetActive(true);
            HUD.SetActive(false);
            if (correctCount == totalHouse)
            {
                levelFinish.SetStars(3);
            } else if (correctCount > wrongCount)
            {
                levelFinish.SetStars(2);
            } else // More wrong than correct
            {
                levelFinish.SetStars(1);
            }
        }
    }

    public void AddCorrect()
    {
        correctCount++;
    }

    public void AddWrong() { 
        wrongCount++;
    }
}
