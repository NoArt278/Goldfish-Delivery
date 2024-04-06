using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private List<House> houses;
    private List<Goldfish> goldfishes;
    private int correctCount, wrongCount;
    [SerializeField] private List<Package> packages;
    [SerializeField] Transform houseParent, goldfishParent;

    public Goldfish selectedGoldfish;
 
    private void Awake()
    {
        selectedGoldfish = null;
        houses = new List<House>();
        goldfishes = new List<Goldfish>();
        for (int i=0; i < houseParent.childCount; i++)
        {
            houses.Add(houseParent.GetChild(i).GetComponent<House>());
            houses[i].levelManager = this;
            houses[i].SetWantedPackage(packages[i]);
        }
        for (int i = 0; i < goldfishParent.childCount; i++)
        {
            goldfishes.Add(goldfishParent.GetChild(i).GetComponent<Goldfish>());
            goldfishes[i].levelManager = this;
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
