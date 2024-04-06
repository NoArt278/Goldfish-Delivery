using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    private Package wantedPackage;
    private Goldfish currSender;
    [HideInInspector] public LevelManager levelManager;

    public bool isDone;

    private void Awake()
    {
        isDone = false;
    }
    public void SetWantedPackage(Package p)
    {
        wantedPackage = p;
    }

    public void SetSender(Goldfish sender)
    {
        currSender = sender;
    }

    private void OnMouseEnter()
    {
        if (!isDone && levelManager.selectedGoldfish != null)
        {
            // Select this house when mouse goes over this house
            levelManager.selectedGoldfish.SelectHouse(this);
        }
    }

    private void OnMouseOver()
    {
        if (!isDone && levelManager.selectedGoldfish != null)
        {
            // Fix line end position to this house
            levelManager.selectedGoldfish.GetComponent<LineRenderer>().SetPosition(1, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject == currSender.gameObject)
        //{
        //    isDone = true;
        //    currSender.RemovePackage();
        //    levelManager.RemoveHouse(this);
        //    if (currSender.GeCurrentPackage().num == wantedPackage.num)
        //    {
        //        /* Add points and play correct animation */
        //        levelManager.AddCorrect();
        //    } else
        //    {
        //        /* Add to mistake count and play wrong animation */
        //        levelManager.AddWrong();
        //    }
        //}
    }
}
