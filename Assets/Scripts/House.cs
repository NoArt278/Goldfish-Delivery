using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    private Package wantedPackage;
    private Goldfish currSender;
    [SerializeField] private PackageBubble packageBubble;
    [SerializeField] private Sprite wrong, correct;
    [HideInInspector] public LevelManager levelManager;

    [HideInInspector] public bool isDone;

    private void Awake()
    {
        isDone = false;
    }
    public void SetWantedPackage(Package p)
    {
        wantedPackage = p;
        packageBubble.Show();
        packageBubble.SetPackageSprite(p.GetComponent<SpriteRenderer>().sprite, p.GetComponent<SpriteRenderer>().color);
        StartCoroutine(ClosePackageBubble());
    }

    IEnumerator ClosePackageBubble()
    {
        yield return new WaitForSeconds(5f);
        packageBubble.Hide();
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (currSender != null && collision.gameObject == currSender.gameObject)
        {
            isDone = true;
            if (currSender.GetCurrentPackage().num == wantedPackage.num)
            {
                /* Add points and play correct animation */
                levelManager.AddCorrect();
                packageBubble.SetPackageSprite(correct, Color.white);
            }
            else
            {
                /* Add to mistake count and play wrong animation */
                levelManager.AddWrong();
                packageBubble.SetPackageSprite(wrong, Color.white);
            }
            packageBubble.Show();
            currSender.RemovePackage();
            currSender = null;
            levelManager.RemoveHouse(this);
        }
    }
}
