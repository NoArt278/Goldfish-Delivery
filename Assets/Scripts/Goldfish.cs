using System.Collections.Generic;
using UnityEngine;

public class Goldfish : MonoBehaviour
{
    private List<Package> heldPackages;
    private LineRenderer lr;
    private House destHouse;
    private const float moveSpeed = 0.5f;
    private Rigidbody2D rb;
    [HideInInspector] public LevelManager levelManager;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        heldPackages = new List<Package>();
        lr.enabled = false;
        int chosenIdx = Random.Range(0, levelManager.GetHouseList().Count);
        SelectHouse(levelManager.GetHouseList()[chosenIdx]);
    }

    public void AddPackage(Package p)
    {
        heldPackages.Add(p);
    }

    public void SelectHouse(House h)
    {
        if (destHouse != null)
        {
            destHouse.SetSender(null);
        }
        destHouse = h;
        destHouse.SetSender(this);
    }

    public Package GeCurrentPackage()
    {
        return heldPackages[^1];
    }

    public void RemovePackage()
    {
        heldPackages.RemoveAt(heldPackages.Count - 1);
        int chosenIdx = Random.Range(0, levelManager.GetHouseList().Count);
        destHouse = levelManager.GetHouseList()[chosenIdx];
    }

    private void OnMouseDown()
    {
        lr.enabled = true;
        levelManager.selectedGoldfish = this;
    }

    private void OnMouseDrag()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void OnMouseUp()
    {
        lr.enabled = false;
        levelManager.selectedGoldfish = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(destHouse.transform.position);
        rb.velocity = transform.forward * moveSpeed;
    }
}
