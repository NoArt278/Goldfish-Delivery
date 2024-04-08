using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private List<House> houses;
    private List<Goldfish> goldfishes;
    [SerializeField] private List<Package> packages;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] Transform houseParent, goldfishParent;

    public int correctCount, wrongCount;
    public bool playDialogue;
    public Goldfish selectedGoldfish;
 
    private void Start()
    {
        if (playDialogue)
        {
            dialogueBox.SetActive(true);
            Time.timeScale = 0;
        }
        selectedGoldfish = null;
        houses = new List<House>();
        goldfishes = new List<Goldfish>();
        int packageCount = packages.Count;
        List<Package> tempPackages = new List<Package>();
        foreach (Package p in packages)
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
        for (int i = 0; i < goldfishParent.childCount; i++)
        {
            int endIdx = packageCount / goldfishParent.childCount;
            if (i == goldfishParent.childCount-1)
            {
                // If last fish, assign all remaining packages
                endIdx = packages.Count;
            }
            goldfishes.Add(goldfishParent.GetChild(i).GetComponent<Goldfish>());
            goldfishes[i].levelManager = this;
            
            for (int j=0; j < endIdx; j++)
            {
                int chosenIdx = Random.Range(0, packages.Count);
                goldfishes[i].AddPackage(packages[chosenIdx]);
                packages.Remove(packages[chosenIdx]);
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
    }

    public void AddCorrect()
    {
        correctCount++;
    }

    public void AddWrong() { 
        wrongCount++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
